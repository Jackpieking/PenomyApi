using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.App.FeatArt4;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt4.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt4.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt4;

public class Art4Endpoint : Endpoint<Art4RequestDto, Art4HttpResponse>
{
    private readonly Lazy<ISnowflakeIdGenerator> _idGenerator;

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

        // Check if input categories has valid json schema or not.
        if (!requestDto.IsValidSelectedCategoriesInput())
        {
            httpResponse = Art4HttpResponseManager
                .Resolve(Art4ResponseStatusCode.INVALID_JSON_SCHEMA_FROM_INPUT_CATEGORIES)
                .Invoke(default);

            await SendAsync(httpResponse, httpResponse.HttpCode, ct);

            return httpResponse;
        }

        // Check if the upload thumbnail image is valid or not.
        if (!requestDto.IsValidThumbnailImageFileInput())
        {
            httpResponse = Art4HttpResponseManager
                .Resolve(Art4ResponseStatusCode.INVALID_FILE_EXTENSION)
                .Invoke(default);

            await SendAsync(httpResponse, httpResponse.HttpCode, ct);

            return httpResponse;
        }

        // Generate a random id for comic.
        var comicId = _idGenerator.Value.Get();
        const long testUserId = 123456789012345678;

        var featArt4Request = requestDto.MapToFeatureRequest(comicId, testUserId);

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
}
