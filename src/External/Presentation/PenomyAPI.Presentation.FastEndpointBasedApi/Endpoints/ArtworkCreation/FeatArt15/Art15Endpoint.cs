using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.App.FeatArt15;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Domain.RelationalDb.Entities.Contraints.ArtworkCreation;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt15.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt15.HttpResponses;
using PenomyAPI.Presentation.FastEndpointBasedApi.Helpers.IFormFiles;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt15;

public class Art15Endpoint : Endpoint<Art15RequestDto, Art15HttpResponse>
{
    private readonly Lazy<ISnowflakeIdGenerator> _idGenerator;
    private static readonly IFormFileHelper _formFileHelper;

    static Art15Endpoint()
    {
        _formFileHelper = FormFileHelper.Instance;
    }

    public Art15Endpoint(Lazy<ISnowflakeIdGenerator> idGenerator)
    {
        _idGenerator = idGenerator;
    }

    public override void Configure()
    {
        Post("art15/anime/create");

        AllowFormData();
        AllowFileUploads();

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<Art15RequestDto>>();
        PreProcessor<ArtworkCreationPreProcessor<Art15RequestDto>>();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for creating a new anime";
            summary.Description = "This endpoint is used for creating new anime purpose.";
            summary.Response<Art15HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = Art15ResponseAppCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<Art15HttpResponse> ExecuteAsync(
        Art15RequestDto requestDto,
        CancellationToken ct
    )
    {
        Art15HttpResponse httpResponse;

        // Check if the upload thumbnail image not empty.
        if (Equals(requestDto.ThumbnailImageFile, null))
        {
            httpResponse = Art15HttpResponse.INVALID_FILE_FORMAT;

            await SendAsync(httpResponse, httpResponse.HttpCode, ct);

            return httpResponse;
        }

        // Check if the upload thumbnail image file is valid or not.
        var validationResult = InternalValidateImageFile(requestDto.ThumbnailImageFile);

        if (!validationResult.IsSuccess)
        {
            httpResponse = validationResult.Value;

            await SendAsync(httpResponse, httpResponse.HttpCode, ct);

            return httpResponse;
        }

        // Check if input categories has valid json schema or not.
        if (!requestDto.IsValidSelectedCategoriesInput())
        {
            httpResponse = Art15HttpResponse.INVALID_JSON_SCHEMA_FROM_INPUT_CATEGORIES;

            await SendAsync(httpResponse, httpResponse.HttpCode, ct);

            return httpResponse;
        }

        // Get the state bag contains creatorId extracted from the access-token.
        var stateBag = ProcessorState<StateBag>();

        long creatorId = stateBag.AppRequest.UserId;

        // Generate a random id for comic.
        var artworkId = _idGenerator.Value.Get();

        var request = requestDto.MapToFeatureRequest(artworkId, creatorId);

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<Art15Request, Art15Response>(
            request,
            ct
        );

        httpResponse = Art15HttpResponse.MapFrom(featResponse);

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
    ///     The <see cref="Result{Art15HttpResponse}"/> instance as the validation result.
    /// </returns>
    private Result<Art15HttpResponse> InternalValidateImageFile(IFormFile imageFile)
    {
        Art15HttpResponse httpResponse;

        // Check if the file extension is valid or not.
        if (!_formFileHelper.HasValidExtension(imageFile, ArtworkConstraints.VALID_IMAGE_FILE_EXTENSIONS))
        {
            httpResponse = Art15HttpResponse.INVALID_FILE_EXTENSION;

            return Result<Art15HttpResponse>.Failed(httpResponse);
        }

        // Check if the uploaded file is really an image file or not.
        if (!_formFileHelper.IsValidImageFile(imageFile))
        {
            httpResponse = Art15HttpResponse.INVALID_FILE_FORMAT;

            return Result<Art15HttpResponse>.Failed(httpResponse);
        }

        // Check if the uploaded file is exceed the size limit or not.
        if (imageFile.Length > ArtworkConstraints.MAXIMUM_IMAGE_FILE_SIZE)
        {
            httpResponse = Art15HttpResponse.FILE_SIZE_IS_EXCEED_THE_LIMIT;

            return Result<Art15HttpResponse>.Failed(httpResponse);
        }

        return Result<Art15HttpResponse>.Success(default);
    }
}
