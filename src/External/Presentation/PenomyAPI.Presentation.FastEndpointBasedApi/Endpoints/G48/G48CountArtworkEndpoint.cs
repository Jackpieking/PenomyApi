using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.G48;
using PenomyAPI.App.G48.OtherHandlers.CountArtwork;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G48.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G48.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G48.HttpResponse;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G48;

public class G48CountArtworkEndpoint : Endpoint<G48CountArtworkRequestDTOs, G48CountArtworkHttpResponse>
{
    public override void Configure()
    {
        Get("/G48/pagination");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);

        PreProcessor<AuthPreProcessor<G48CountArtworkRequestDTOs>>();

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for user get favorite artworks";
            summary.Description = "This endpoint is used for user get favorite artworks";
            summary.Response<G48HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G48ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G48CountArtworkHttpResponse> ExecuteAsync(
        G48CountArtworkRequestDTOs requestDto,
        CancellationToken ct
    )
    {
        StateBag stateBag = ProcessorState<StateBag>();

        var featRequest = new G48CountArtworkRequest
        {
            UserId = stateBag.AppRequest.UserId,
            ArtworkType = requestDto.ArtworkType,
        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<G48CountArtworkRequest, G48CountArtworkResponse>(
            featRequest,
            ct
        );

        var httpResponse = new G48CountArtworkHttpResponse
        {
            HttpCode = StatusCodes.Status200OK,
            Body = new G48PaginationOptions
            {
                AllowPagination = false,
                TotalPages = 0
            }
        };

        if (featResponse.TotalArtwork > 0)
        {
            httpResponse.Body.AllowPagination = true;
            httpResponse.Body.TotalArtworks = featResponse.TotalArtwork;
            httpResponse.Body.TotalPages = (int)Math.Ceiling((decimal)featResponse.TotalArtwork / G48PaginationOptions.DEFAULT_PAGE_SIZE);
        }

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
