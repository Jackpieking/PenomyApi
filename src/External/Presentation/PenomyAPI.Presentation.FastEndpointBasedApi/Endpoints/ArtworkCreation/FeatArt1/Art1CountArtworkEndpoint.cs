using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt1;
using PenomyAPI.App.FeatArt1.OtherHandlers.CountArtwork;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt1.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt1.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt1.HttpResponse;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt1;

public sealed class Art1CountArtworkEndpoint
    : Endpoint<Art1CountArtworkRequestDto, Art1CountArtworkHttpResponse>
{
    public override void Configure()
    {
        Get("art1/pagination");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<Art1CountArtworkRequestDto>>();
        PreProcessor<ArtworkCreationPreProcessor<Art1CountArtworkRequestDto>>();

        Description(builder =>
        {
            builder.ClearDefaultProduces(StatusCodes.Status400BadRequest);
            builder.ClearDefaultProduces(StatusCodes.Status401Unauthorized);
            builder.ClearDefaultProduces(StatusCodes.Status403Forbidden);
        });

        Summary(summary =>
        {
            summary.Summary = "Endpoint for getting the pagination options for get studio artworks page.";
            summary.Description = "This endpoint is used to get the pagination options for get studio artworks page.";
            summary.ExampleRequest = new() { };
            summary.Response<Art1HttpResponse>(
                description: "Represent successful operation response.",
                example: new()
                {
                    HttpCode = StatusCodes.Status200OK,
                    AppCode = $"ART1.{Art1ResponseAppCode.SUCCESS}"
                }
            );
        });
    }

    public override async Task<Art1CountArtworkHttpResponse> ExecuteAsync(
        Art1CountArtworkRequestDto requestDto,
        CancellationToken ct
    )
    {
        // Get the state bag contains userId extracted from the access-token.
        var stateBag = ProcessorState<StateBag>();

        long creatorId = stateBag.AppRequest.UserId;

        var request = requestDto.MapToRequest(creatorId);

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
        var totalPages = Math.Ceiling((double) featResponse.TotalArtworks / pageSize);

        if (totalPages > 0)
        {
            httpResponse.Body.AllowPagination = true;
            httpResponse.Body.TotalPages = (int) totalPages;
            httpResponse.Body.TotalArtworks = featResponse.TotalArtworks;
        }

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
