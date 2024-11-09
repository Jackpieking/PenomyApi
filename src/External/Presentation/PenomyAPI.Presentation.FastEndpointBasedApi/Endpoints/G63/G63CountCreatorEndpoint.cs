using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.G63;
using PenomyAPI.App.G63.OtherHandlers.CountArtwork;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G63.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G63.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G63.HttpResponse;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G63;

public class G63CountCreatorEndpoint : Endpoint<G63CountCreatorRequestDto, G63CountArtworkHttpResponse>
{
    public override void Configure()
    {
        Get("/G63/pagination");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);

        PreProcessor<AuthPreProcessor<G63CountCreatorRequestDto>>();

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for user get followed creators";
            summary.Description = "This endpoint is used for user get followed creators";
            summary.Response<G63HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G63ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G63CountArtworkHttpResponse> ExecuteAsync(
        G63CountCreatorRequestDto requestDto,
        CancellationToken ct
    )
    {
        StateBag stateBag = ProcessorState<StateBag>();

        var featRequest = new G63CountArtworkRequest
        {
            UserId = stateBag.AppRequest.UserId
        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<G63CountArtworkRequest, G63CountArtworkResponse>(
            featRequest,
            ct
        );

        var httpResponse = new G63CountArtworkHttpResponse
        {
            HttpCode = StatusCodes.Status200OK,
            Body = new G63PaginationOptions
            {
                AllowPagination = false,
                TotalPages = 0
            }
        };

        if (featResponse.TotalArtwork > 0)
        {
            httpResponse.Body.AllowPagination = true;
            httpResponse.Body.TotalArtworks = featResponse.TotalArtwork;
            httpResponse.Body.TotalPages = (int)Math.Ceiling((decimal)featResponse.TotalArtwork / G63PaginationOptions.DEFAULT_PAGE_SIZE);
        }

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
