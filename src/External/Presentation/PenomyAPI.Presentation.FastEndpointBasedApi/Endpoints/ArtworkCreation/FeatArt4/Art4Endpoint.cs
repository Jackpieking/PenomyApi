using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.App.FeatArt4;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Domain.RelationalDb.Entities.Contraints.ArtworkCreation;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt4.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt4.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Helpers.IFormFiles;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt4;

public class Art4Endpoint : Endpoint<Art4RequestDto, Art4HttpResponse>
{
    private readonly Lazy<ISnowflakeIdGenerator> _idGenerator;
    private static readonly IFormFileHelper _formFileHelper;

    static Art4Endpoint()
    {
        _formFileHelper = FormFileHelper.Instance;
    }

    public Art4Endpoint(Lazy<ISnowflakeIdGenerator> idGenerator)
    {
        _idGenerator = idGenerator;

        var id = _idGenerator.Value.Get();
    }

    public override void Configure()
    {
        Post("/art4/comic/create");

        AllowAnonymous();
        AllowFormData();
        AllowFileUploads();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for creating a new comic";
            summary.Description = "This endpoint is used for creating new comic purpose.";
            summary.Response<Art4HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = Art4ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<Art4HttpResponse> ExecuteAsync(
        Art4RequestDto requestDto,
        CancellationToken ct
    )
    {
        Art4HttpResponse httpResponse;

        // Check if the upload thumbnail image not empty.
        if (Equals(requestDto.ThumbnailImageFile, null))
        {
            httpResponse = Art4HttpResponseManager
                .Resolve(Art4ResponseStatusCode.INVALID_FILE_FORMAT)
                .Invoke(default);

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
            httpResponse = Art4HttpResponseManager
                .Resolve(Art4ResponseStatusCode.INVALID_JSON_SCHEMA_FROM_INPUT_CATEGORIES)
                .Invoke(default);

            await SendAsync(httpResponse, httpResponse.HttpCode, ct);

            return httpResponse;
        }

        // Generate a random id for comic.
        var comicId = _idGenerator.Value.Get();
        const long testCreatorId = 123456789012345678;

        var featArt4Request = requestDto.MapToFeatureRequest(comicId, testCreatorId);

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<Art4Request, Art4Response>(
            featArt4Request,
            ct
        );

        httpResponse = Art4HttpResponseManager
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
    ///     The <see cref="Result{Art4HttpResponse}"/> instance as the validation result.
    /// </returns>
    private Result<Art4HttpResponse> InternalValidateImageFile(IFormFile imageFile)
    {
        Art4HttpResponse httpResponse;

        // Check if the file extension is valid or not.
        if (!_formFileHelper.HasValidExtension(imageFile, ArtworkConstraints.VALID_FILE_EXTENSIONS))
        {
            httpResponse = Art4HttpResponseManager
                .Resolve(Art4ResponseStatusCode.INVALID_FILE_EXTENSION)
                .Invoke(default);

            return Result<Art4HttpResponse>.Failed(httpResponse);
        }

        // Check if the uploaded file is really an image file or not.
        if (!_formFileHelper.IsValidImageFile(imageFile))
        {
            httpResponse = Art4HttpResponseManager
                .Resolve(Art4ResponseStatusCode.INVALID_FILE_FORMAT)
                .Invoke(default);

            return Result<Art4HttpResponse>.Failed(httpResponse);
        }

        // Check if the uploaded file is exceed the size limit or not.
        if (imageFile.Length > ArtworkConstraints.MAXIMUM_IMAGE_FILE_SIZE)
        {
            httpResponse = Art4HttpResponseManager
                .Resolve(Art4ResponseStatusCode.FILE_SIZE_IS_EXCEED_THE_LIMIT)
                .Invoke(default);

            return Result<Art4HttpResponse>.Failed(httpResponse);
        }

        return Result<Art4HttpResponse>.Success(default);
    }
}
