using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt4;
using PenomyAPI.App.FeatArt7;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt4.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt7.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt7.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt7.HttpResponseManagers;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt7;

public sealed class Art7Endpoint : Endpoint<Art7RequestDto, Art7HttpResponse>
{
    public override void Configure()
    {
        Patch("art7/edit");

        AllowAnonymous();
        AllowFileUploads();
        AllowFormData();

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

    public override async Task<Art7HttpResponse> ExecuteAsync(Art7RequestDto requestDto, CancellationToken ct)
    {
        Art7HttpResponse httpResponse;

        // Check if input categories has valid json schema or not.
        if (!requestDto.IsValidSelectedCategoriesInput())
        {
            httpResponse = Art7HttpResponseManager
                .Resolve(Art7ResponseStatusCode.INVALID_JSON_SCHEMA_FROM_INPUT_CATEGORIES)
                .Invoke(default);

            await SendAsync(httpResponse, httpResponse.HttpCode, ct);

            return httpResponse;
        }

        // If thumbnail image has update, then check if it is valid or not.
        if (requestDto.IsThumbnailUpdated() && !requestDto.IsValidThumbnailImageFileInput())
        {
            httpResponse = Art7HttpResponseManager
                .Resolve(Art7ResponseStatusCode.INVALID_FILE_EXTENSION)
                .Invoke(default);

            await SendAsync(httpResponse, httpResponse.HttpCode, ct);

            return httpResponse;
        }

        var testUserId = 123456789012345678;

        var featArt7Request = requestDto.MapToFeatureRequest(testUserId);

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
}
