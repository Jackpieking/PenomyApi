using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.Common.FileServices;
using PenomyAPI.App.Common.FileServices.Models;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.App.SM8;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Domain.RelationalDb.Entities.Contraints.SocialGroupCreation;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM8.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM8.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM8.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM8.Middlewares.Authorization;
using PenomyAPI.Presentation.FastEndpointBasedApi.Helpers.IFormFiles;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM8;

public class SM8Endpoint : Endpoint<SM8RequestDto, SM8HttpResponse>
{
    private static readonly IFormFileHelper _formFileHelper;

    static SM8Endpoint()
    {
        _formFileHelper = FormFileHelper.Instance;
    }

    public override void Configure()
    {
        Post("sm8/groups/create");
        DontThrowIfValidationFails();
        AllowFormData();
        AllowFileUploads();
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<SM8AuthorizationPreProcessor>();
        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for creating groups.";
            summary.Description = "This endpoint is used for creating groups.";
            summary.Response<SM8HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = SM8ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<SM8HttpResponse> ExecuteAsync(
        SM8RequestDto req,
        CancellationToken ct
    )
    {
        SM8HttpResponse httpResponse;

        // Check if the upload cover image not empty.
        if (Equals(req.CoverPhoto, null))
        {
            httpResponse = SM8HttpResponseManager
                .Resolve(SM8ResponseStatusCode.INVALID_FILE_FORMAT)
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

        var stateBag = ProcessorState<SM8StateBag>();

        var SM8Request = new SM8Request
        {
            GroupName = req.Name,
            IsPublic = req.IsPublic,
            Description = req.Description,
            RequireApprovedWhenPost = req.RequireApprovedWhenPost,
            CoverImageFileInfo = new ImageFileInfo{
                FileName = req.CoverPhoto.FileName,
                FileDataStream = req.CoverPhoto.OpenReadStream(),
            }
        };

        SM8Request.SetUserId(stateBag.AppRequest.GetUserId());

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<SM8Request, SM8Response>(
            SM8Request,
            ct
        );

        httpResponse = SM8HttpResponseManager.Resolve(featResponse.StatusCode).Invoke(featResponse);

        if (featResponse.IsSuccess)
        {
            httpResponse.Body = new SM8ResponseDto { GroupId = featResponse.Result.ToString() };

            return httpResponse;
        }

        return httpResponse;
    }

    /// <summary>
    ///     Internally validate the input <paramref name="imageFile"/>,
    /// </summary>
    /// <param name="imageFile">
    ///     The image file to validate.
    /// </param>
    /// <returns>
    ///     The <see cref="Result{SM8HttpResponse}"/> instance as the validation result.
    /// </returns>
    private Result<SM8HttpResponse> InternalValidateImageFile(IFormFile imageFile)
    {
        SM8HttpResponse httpResponse;

        // Check if the file extension is valid or not.
        if (!_formFileHelper.HasValidExtension(imageFile, GroupConstraints.VALID_FILE_EXTENSIONS))
        {
            httpResponse = SM8HttpResponseManager
                .Resolve(SM8ResponseStatusCode.INVALID_FILE_EXTENSION)
                .Invoke(default);

            return Result<SM8HttpResponse>.Failed(httpResponse);
        }

        // Check if the uploaded file is really an image file or not.
        if (!_formFileHelper.IsValidImageFile(imageFile))
        {
            httpResponse = SM8HttpResponseManager
                .Resolve(SM8ResponseStatusCode.INVALID_FILE_FORMAT)
                .Invoke(default);

            return Result<SM8HttpResponse>.Failed(httpResponse);
        }

        // Check if the uploaded file is exceed the size limit or not.
        if (imageFile.Length > GroupConstraints.MAXIMUM_IMAGE_FILE_SIZE)
        {
            httpResponse = SM8HttpResponseManager
                .Resolve(SM8ResponseStatusCode.MAXIMUM_IMAGE_FILE_SIZE)
                .Invoke(default);

            return Result<SM8HttpResponse>.Failed(httpResponse);
        }

        return Result<SM8HttpResponse>.Success(default);
    }
}
