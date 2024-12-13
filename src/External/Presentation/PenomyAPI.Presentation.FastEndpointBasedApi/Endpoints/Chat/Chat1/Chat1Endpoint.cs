using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.Common.FileServices.Models;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.App.FeatChat1;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Domain.RelationalDb.Entities.Contraints.SocialGroupCreation;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Chat.Chat1.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Chat.Chat1.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Helpers.IFormFiles;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Chat.Chat1;

public class Chat1Endpoint : Endpoint<Chat1RequestDto, Chat1HttpResponse>
{
    private static readonly IFormFileHelper _formFileHelper;

    static Chat1Endpoint()
    {
        _formFileHelper = FormFileHelper.Instance;
    }

    public override void Configure()
    {
        Post("Chat1/chat-groups/create");
        DontThrowIfValidationFails();
        AllowFormData();
        AllowFileUploads();
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<Chat1RequestDto>>();
        Description(builder => { builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest); });

        Summary(summary =>
        {
            summary.Summary = "Endpoint for creating chat groups.";
            summary.Description = "This endpoint is used for creating chat groups.";
            summary.Response(
                description: "Represent successful operation response.",
                example: new Chat1HttpResponse { AppCode = Chat1ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<Chat1HttpResponse> ExecuteAsync(
        Chat1RequestDto req,
        CancellationToken ct
    )
    {
        var stateBag = ProcessorState<StateBag>();
        Chat1HttpResponse httpResponse;

        // Check if the upload cover image not empty.
        if (Equals(req.CoverPhoto, null))
        {
            httpResponse = Chat1HttpResponseManager
                .Resolve(Chat1ResponseStatusCode.INVALID_FILE_FORMAT)
                .Invoke(default);

            await SendAsync(httpResponse, httpResponse.HttpCode, ct);

            return httpResponse;
        }

        // Check if the upload cover image file is valid or not.
        var validationResult = InternalValidateImageFile(req.CoverPhoto);

        if (!validationResult.IsSuccess)
        {
            httpResponse = validationResult.Value;

            await SendAsync(httpResponse, httpResponse.HttpCode, ct);

            return httpResponse;
        }

        var Chat1Request = new Chat1Request
        {
            GroupName = req.GroupName,
            IsPublic = req.IsPublic,
            UserId = stateBag.AppRequest.UserId,
            CoverImageFileInfo = new ImageFileInfo
            {
                FileName = req.CoverPhoto.FileName,
                FileDataStream = req.CoverPhoto.OpenReadStream()
            },
            GroupType = req.GroupType
        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<Chat1Request, Chat1Response>(
            Chat1Request,
            ct
        );

        httpResponse = Chat1HttpResponseManager.Resolve(featResponse.StatusCode).Invoke(featResponse);

        if (featResponse.IsSuccess)
        {
            httpResponse.Body = new Chat1ResponseDto { GroupId = featResponse.Result.ToString() };

            return httpResponse;
        }

        return httpResponse;
    }

    /// <summary>
    ///     Internally validate the input <paramref name="imageFile" />,
    /// </summary>
    /// <param name="imageFile">
    ///     The image file to validate.
    /// </param>
    /// <returns>
    ///     The <see cref="Result{TValue}" /> instance as the validation result.
    /// </returns>
    private Result<Chat1HttpResponse> InternalValidateImageFile(IFormFile imageFile)
    {
        Chat1HttpResponse httpResponse;

        // Check if the file extension is valid or not.
        if (!_formFileHelper.HasValidExtension(imageFile, GroupConstraints.VALID_FILE_EXTENSIONS))
        {
            httpResponse = Chat1HttpResponseManager
                .Resolve(Chat1ResponseStatusCode.INVALID_FILE_EXTENSION)
                .Invoke(default);

            return Result<Chat1HttpResponse>.Failed(httpResponse);
        }

        // Check if the uploaded file is really an image file or not.
        if (!_formFileHelper.IsValidImageFile(imageFile))
        {
            httpResponse = Chat1HttpResponseManager
                .Resolve(Chat1ResponseStatusCode.INVALID_FILE_FORMAT)
                .Invoke(default);

            return Result<Chat1HttpResponse>.Failed(httpResponse);
        }

        // Check if the uploaded file is exceed the size limit or not.
        if (imageFile.Length > GroupConstraints.MAXIMUM_IMAGE_FILE_SIZE)
        {
            httpResponse = Chat1HttpResponseManager
                .Resolve(Chat1ResponseStatusCode.MAXIMUM_IMAGE_FILE_SIZE)
                .Invoke(default);

            return Result<Chat1HttpResponse>.Failed(httpResponse);
        }

        return Result<Chat1HttpResponse>.Success(default);
    }
}
