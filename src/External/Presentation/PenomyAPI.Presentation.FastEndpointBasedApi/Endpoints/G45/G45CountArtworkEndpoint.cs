using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.G45;
using PenomyAPI.App.G45.OtherHandlers.CountArtwork;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G45.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G45.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G45.HttpResponse;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G45;

public class G45CountArtworkEndpoint : Endpoint<G45CountArtworkRequestDTO, G45CountArtworkHttpResponse>
{
    public override void Configure()
    {
        Get("/G45/pagination");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);

        PreProcessor<AuthPreProcessor<G45CountArtworkRequestDTO>>();

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for user get total followed artworks";
            summary.Description = "This endpoint is used for user get favorite artworks";
            summary.Response<G45HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G45ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G45CountArtworkHttpResponse> ExecuteAsync(
        G45CountArtworkRequestDTO requestDto,
        CancellationToken ct
    )
    {
        StateBag stateBag = ProcessorState<StateBag>();

        var featRequest = new G45CountArtworkRequest
        {
            UserId = stateBag.AppRequest.UserId,
            ArtworkType = requestDto.ArtworkType,
        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<G45CountArtworkRequest, G45CountArtworkResponse>(
            featRequest,
            ct
        );

        var httpResponse = new G45CountArtworkHttpResponse
        {
            HttpCode = StatusCodes.Status200OK,
            Body = new G45PaginationOptions
            {
                AllowPagination = false,
                TotalPages = 0
            }
        };

        if (featResponse.TotalArtwork > 0)
        {
            httpResponse.Body.AllowPagination = true;
            httpResponse.Body.TotalArtworks = featResponse.TotalArtwork;
            httpResponse.Body.TotalPages = (int)Math.Ceiling((decimal)featResponse.TotalArtwork / G45PaginationOptions.DEFAULT_PAGE_SIZE);
        }

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
