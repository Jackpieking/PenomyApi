using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt1.OtherHandlers;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt1.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt1.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt1;

public sealed class Art1CountArtworkEndpoint
    : Endpoint<Art1CountArtworkRequest, Art1CountArtworkHttpResponse>
{
    public override void Configure()
    {
        Get("art1/count-artwork");

        AllowAnonymous();
    }

    public override async Task<Art1CountArtworkHttpResponse> ExecuteAsync(
        Art1CountArtworkRequest request,
        CancellationToken ct
    )
    {
        var featResponse = await FeatureExtensions.ExecuteAsync<
            Art1CountArtworkRequest,
            Art1CountArtworkResponse
        >(request, ct);

        var httpResponse = new Art1CountArtworkHttpResponse
        {
            HttpCode = StatusCodes.Status200OK,
            Body = new Art1PaginationOptions
            {
                AllowPagination = false,
                TotalPages = 0
            }
        };

        // Get all the artworks and calculate the pagination.
        const int pageSize = Art1PaginationOptions.DEFAULT_PAGE_SIZE;
        var totalPages = Math.Ceiling((double)featResponse.TotalArtworks / pageSize);
        if (totalPages > 0)
        {
            httpResponse.Body.AllowPagination = true;
            httpResponse.Body.TotalPages = (int)totalPages;
        }

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
