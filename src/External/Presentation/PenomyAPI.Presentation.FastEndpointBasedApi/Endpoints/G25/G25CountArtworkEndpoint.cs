using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.G25;
using PenomyAPI.App.G25.OtherHandlers.CountArtwork;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25.HttpResponse;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25;

public class G25CountArtworkEndpoint
    : Endpoint<G25CountArtworkRequestDto, G25CountArtworkHttpResponse>
{
    public override void Configure()
    {
        Get("/G25/profile/user/history/count");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);

        PreProcessor<AuthPreProcessor<G25CountArtworkRequestDto>>();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });
        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for get number of artworks viewed";
            summary.Description = "This endpoint is used for get number of artworks viewed.";
            summary.Response<G25HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G25ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }


    public override async Task<G25CountArtworkHttpResponse> ExecuteAsync(
        G25CountArtworkRequestDto request,
        CancellationToken ct
    )
    {
        var stateBag = ProcessorState<StateBag>();

        var featRequest = new G25CountArtworkRequest
        {
            UserId = stateBag.AppRequest.UserId,
            ArtworkType = request.ArtworkType
        };

        var featResponse = await FeatureExtensions.ExecuteAsync<
            G25CountArtworkRequest,
            G25CountArtworkResponse
        >(featRequest, ct);

        var httpResponse = new G25CountArtworkHttpResponse
        {
            HttpCode = StatusCodes.Status200OK,
            Body = new G25PaginationOptions
            {
                AllowPagination = false,
                TotalPages = 0
            }
        };

        if (featResponse.TotalArtwork > 0)
        {
            httpResponse.Body.AllowPagination = true;
            httpResponse.Body.TotalArtworks = featResponse.TotalArtwork;
            httpResponse.Body.TotalPages = (int)Math.Ceiling((decimal)featResponse.TotalArtwork / G25PaginationOptions.DEFAULT_PAGE_SIZE);
        }

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}