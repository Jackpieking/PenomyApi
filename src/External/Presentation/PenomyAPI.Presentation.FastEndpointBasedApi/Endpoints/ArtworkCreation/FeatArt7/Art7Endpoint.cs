using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.App.FeatArt4;
using PenomyAPI.App.FeatArt7;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Domain.RelationalDb.Entities.Contraints.ArtworkCreation;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt4.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt7.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt7.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt7.HttpResponseManagers;
using PenomyAPI.Presentation.FastEndpointBasedApi.Helpers.IFormFiles;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt7;

public sealed class Art7Endpoint : Endpoint<Art7RequestDto, Art7HttpResponse>
{
    private static readonly IFormFileHelper _formFileHelper;

    static Art7Endpoint()
    {
        _formFileHelper = FormFileHelper.Instance;
    }

    public override void Configure()
    {
        Patch("art7/comic/edit");

        AllowFileUploads();
        AllowFormData();

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<Art7RequestDto>>();
        PreProcessor<ArtworkCreationPreProcessor<Art7RequestDto>>();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for edit a comic";
            summary.Description = "This endpoint is used for editing a comic.";
            summary.Response<Art4HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = Art4ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<Art7HttpResponse> ExecuteAsync(
        Art7RequestDto requestDto,
        CancellationToken ct
    )
    {
        Art7HttpResponse httpResponse;

        // Check if the thumbnail is updated or not.
        if (requestDto.IsThumbnailUpdated())
        {
            var validationResult = InternalValidateImageFile(requestDto.ThumbnailImageFile);

            if (!validationResult.IsSuccess)
            {
                httpResponse = validationResult.Value;

                await SendAsync(httpResponse, httpResponse.HttpCode, ct);

                return httpResponse;
            }
        }

        // Check if input categories has valid json schema or not.
        if (requestDto.IsCategoriesUpdated && !requestDto.IsValidSelectedCategoriesInput())
        {
            httpResponse = Art7HttpResponseManager
                .Resolve(Art7ResponseStatusCode.INVALID_JSON_SCHEMA_FROM_INPUT_CATEGORIES)
                .Invoke(default);

            await SendAsync(httpResponse, httpResponse.HttpCode, ct);

            return httpResponse;
        }

        // Get the state bag contains creatorId extracted from the access-token.
        var stateBag = ProcessorState<StateBag>();

        long creatorId = stateBag.AppRequest.UserId;

        var featArt7Request = requestDto.MapToFeatureRequest(creatorId);

        // Execute the handler and get response.
        var featResponse = await FeatureExtensions.ExecuteAsync<Art7Request, Art7Response>(
            featArt7Request,
            ct
        );

        httpResponse = Art7HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(featResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }

    /// <summary>
    ///     Internally validate the input <paramref name="imageFile"/>,
    /// </summary>
    /// <param name="imageFile">
    ///     The image file to validate.
    /// </param>
    /// <returns>
    ///     The <see cref="Result{Art7HttpResponse}"/> instance as the validation result.
    /// </returns>
    private Result<Art7HttpResponse> InternalValidateImageFile(IFormFile imageFile)
    {
        Art7HttpResponse httpResponse;

        // Check if the file extension is valid or not.
        if (!_formFileHelper.HasValidExtension(imageFile, ArtworkConstraints.VALID_IMAGE_FILE_EXTENSIONS))
        {
            httpResponse = Art7HttpResponseManager
                .Resolve(Art7ResponseStatusCode.INVALID_FILE_EXTENSION)
                .Invoke(default);

            return Result<Art7HttpResponse>.Failed(httpResponse);
        }

        // Check if the uploaded file is really an image file or not.
        if (!_formFileHelper.IsValidImageFile(imageFile))
        {
            httpResponse = Art7HttpResponseManager
                .Resolve(Art7ResponseStatusCode.INVALID_FILE_FORMAT)
                .Invoke(default);

            return Result<Art7HttpResponse>.Failed(httpResponse);
        }

        // Check if the uploaded file is exceed the size limit or not.
        if (imageFile.Length > ArtworkConstraints.MAXIMUM_IMAGE_FILE_SIZE)
        {
            httpResponse = Art7HttpResponseManager
                .Resolve(Art7ResponseStatusCode.FILE_SIZE_IS_EXCEED_THE_LIMIT)
                .Invoke(default);

            return Result<Art7HttpResponse>.Failed(httpResponse);
        }

        return Result<Art7HttpResponse>.Success(default);
    }
}
