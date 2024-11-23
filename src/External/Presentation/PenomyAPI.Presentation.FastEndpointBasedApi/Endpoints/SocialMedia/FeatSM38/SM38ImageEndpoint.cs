using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.Common.FileServices.Models;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.App.SM38;
using PenomyAPI.App.SM38.CoverImage;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Domain.RelationalDb.Entities.Contraints.SocialGroupCreation;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM38Image.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM38Image.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Helpers.IFormFiles;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM38Image;

public class SM38ImageEndpoint : Endpoint<SM38ImageRequestDto, SM38ImageHttpResponse>
{
    private static readonly IFormFileHelper _formFileHelper;

    static SM38ImageEndpoint()
    {
        _formFileHelper = FormFileHelper.Instance;
    }

    public override void Configure()
    {
        Post("sm38/group-cover-image/update");
        DontThrowIfValidationFails();
        AllowFormData();
        AllowFileUploads();
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<SM38ImageRequestDto>>();
        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for creating groups.";
            summary.Description = "This endpoint is used for creating groups.";
            summary.Response<SM38ImageHttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = SM38ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<SM38ImageHttpResponse> ExecuteAsync(
        SM38ImageRequestDto req,
        CancellationToken ct
    )
    {
        SM38ImageHttpResponse httpResponse;

        // Check if the upload cover image not empty.
        if (Equals(req.CoverPhoto, null))
        {
            httpResponse = SM38ImageHttpResponseManager
                .Resolve(SM38ResponseStatusCode.INVALID_FILE_FORMAT)
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

        var stateBag = ProcessorState<StateBag>();

        var SM38ImageRequest = new SM38ImageRequest
        {
            UserId = stateBag.AppRequest.UserId,
            GroupId = long.Parse(req.GroupId),
            CoverImageFileInfo = new ImageFileInfo
            {
                FileName = req.CoverPhoto.FileName,
                FileDataStream = req.CoverPhoto.OpenReadStream(),
            },
        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<
            SM38ImageRequest,
            SM38ImageResponse
        >(SM38ImageRequest, ct);

        httpResponse = SM38ImageHttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(featResponse);

        if (featResponse.IsSuccess)
        {
            httpResponse.Body = new SM38ImageResponseDto { ImageUrl = featResponse.Result };

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
    ///     The <see cref="Result{SM38ImageHttpResponse}"/> instance as the validation result.
    /// </returns>
    private Result<SM38ImageHttpResponse> InternalValidateImageFile(IFormFile imageFile)
    {
        SM38ImageHttpResponse httpResponse;

        // Check if the file extension is valid or not.
        if (!_formFileHelper.HasValidExtension(imageFile, GroupConstraints.VALID_FILE_EXTENSIONS))
        {
            httpResponse = SM38ImageHttpResponseManager
                .Resolve(SM38ResponseStatusCode.INVALID_FILE_EXTENSION)
                .Invoke(default);

            return Result<SM38ImageHttpResponse>.Failed(httpResponse);
        }

        // Check if the uploaded file is really an image file or not.
        if (!_formFileHelper.IsValidImageFile(imageFile))
        {
            httpResponse = SM38ImageHttpResponseManager
                .Resolve(SM38ResponseStatusCode.INVALID_FILE_FORMAT)
                .Invoke(default);

            return Result<SM38ImageHttpResponse>.Failed(httpResponse);
        }

        // Check if the uploaded file is exceed the size limit or not.
        if (imageFile.Length > GroupConstraints.MAXIMUM_IMAGE_FILE_SIZE)
        {
            httpResponse = SM38ImageHttpResponseManager
                .Resolve(SM38ResponseStatusCode.MAXIMUM_IMAGE_FILE_SIZE)
                .Invoke(default);

            return Result<SM38ImageHttpResponse>.Failed(httpResponse);
        }

        return Result<SM38ImageHttpResponse>.Success(default);
    }
}
