using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt1.OtherHandlers.CountArtwork;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt1.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt1.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt1.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt1;

public sealed class Art1CountArtworkEndpoint
    : Endpoint<Art1CountArtworkRequestDto, Art1CountArtworkHttpResponse>
{
    public override void Configure()
    {
        Get("art1/pagination");

        AllowAnonymous();
    }

    public override async Task<Art1CountArtworkHttpResponse> ExecuteAsync(
        Art1CountArtworkRequestDto requestDto,
        CancellationToken ct
    )
    {
        var testCreatorId = 123456789012345678;
        var request = requestDto.MapToRequest(testCreatorId);

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
            httpResponse.Body.TotalArtworks = featResponse.TotalArtworks;
        }

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
