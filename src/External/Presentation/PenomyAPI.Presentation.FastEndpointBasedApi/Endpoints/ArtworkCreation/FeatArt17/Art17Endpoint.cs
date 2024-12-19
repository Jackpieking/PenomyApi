using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.App.FeatArt17;
using PenomyAPI.App.FeatArt4;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Domain.RelationalDb.Entities.Contraints.ArtworkCreation;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt17.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt17.HttpResponses;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt4.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Helpers.IFormFiles;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt17;

public class Art17Endpoint : Endpoint<Art17EditAnimeRequestDto, Art17HttpResponse>
{
    private static readonly IFormFileHelper _formFileHelper;

    static Art17Endpoint()
    {
        _formFileHelper = FormFileHelper.Instance;
    }

    public override void Configure()
    {
        Patch("art17/anime/edit");

        AllowFileUploads();
        AllowFormData();

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<Art17EditAnimeRequestDto>>();
        PreProcessor<ArtworkCreationPreProcessor<Art17EditAnimeRequestDto>>();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for edit an anime detail";
            summary.Description = "This endpoint is used for editing an anime detail.";
            summary.Response<Art4HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = Art4ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<Art17HttpResponse> ExecuteAsync(
        Art17EditAnimeRequestDto requestDto,
        CancellationToken ct
    )
    {
        Art17HttpResponse httpResponse;

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
            httpResponse = Art17HttpResponse.INVALID_JSON_SCHEMA_FROM_INPUT_CATEGORIES;

            await SendAsync(httpResponse, httpResponse.HttpCode, ct);

            return httpResponse;
        }

        // Get the state bag contains creatorId extracted from the access-token.
        var stateBag = ProcessorState<StateBag>();

        long creatorId = stateBag.AppRequest.UserId;

        var request = requestDto.MapToFeatureRequest(creatorId);

        // Execute the handler and get response.
        var featResponse = await FeatureExtensions.ExecuteAsync<Art17Request, Art17Response>(
            request,
            ct
        );

        httpResponse = Art17HttpResponse.MapFrom(featResponse);

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
    ///     The <see cref="Result{Art17HttpResponse}"/> instance as the validation result.
    /// </returns>
    private Result<Art17HttpResponse> InternalValidateImageFile(IFormFile imageFile)
    {
        Art17HttpResponse httpResponse;

        // Check if the file extension is valid or not.
        if (!_formFileHelper.HasValidExtension(imageFile, ArtworkConstraints.VALID_IMAGE_FILE_EXTENSIONS))
        {
            httpResponse = Art17HttpResponse.INVALID_FILE_EXTENSION;

            return Result<Art17HttpResponse>.Failed(httpResponse);
        }

        // Check if the uploaded file is really an image file or not.
        if (!_formFileHelper.IsValidImageFile(imageFile))
        {
            httpResponse = Art17HttpResponse.INVALID_FILE_FORMAT;

            return Result<Art17HttpResponse>.Failed(httpResponse);
        }

        // Check if the uploaded file is exceed the size limit or not.
        if (imageFile.Length > ArtworkConstraints.MAXIMUM_IMAGE_FILE_SIZE)
        {
            httpResponse = Art17HttpResponse.FILE_SIZE_IS_EXCEED_THE_LIMIT;

            return Result<Art17HttpResponse>.Failed(httpResponse);
        }

        return Result<Art17HttpResponse>.Success(default);
    }
}
