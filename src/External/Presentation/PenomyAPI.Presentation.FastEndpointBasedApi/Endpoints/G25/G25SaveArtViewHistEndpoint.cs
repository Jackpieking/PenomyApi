using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.G25;
using PenomyAPI.App.G25.OtherHandlers.SaveArtViewHist;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25.HttpResponse;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25;

public class G25SaveArtViewHistEndpoint
    : Endpoint<G25SaveArtViewHistRequestDto, G25SaveArtViewHistHttpResponse>
{
    public override void Configure()
    {
        Post("/G25/profile/user/history/save");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);

        PreProcessor<AuthPreProcessor<G25SaveArtViewHistRequestDto>>();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for save chapter view history";
            summary.Description = "This endpoint is used for save chapter view history.";
            summary.Response<G25SaveArtViewHistHttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G25ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G25SaveArtViewHistHttpResponse> ExecuteAsync(
        G25SaveArtViewHistRequestDto request,
        CancellationToken ct
    )
    {
        var stateBag = ProcessorState<StateBag>();

        var featRequest = new G25SaveArtViewHistRequest
        {
            UserId = stateBag.AppRequest.UserId,
            ArtworkId = request.ArtworkId,
            ChapterId = request.ChapterId,
            ArtworkType = request.ArtworkType
        };

        var featResponse = await FeatureExtensions.ExecuteAsync<
            G25SaveArtViewHistRequest,
            G25SaveArtViewHistResponse>(
                featRequest,
                ct
            );

        G25SaveArtViewHistHttpResponse httpResponse = G25SaveArtViewHistResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(featRequest, featResponse);

        if (featResponse.IsSuccess)
        {
            httpResponse.Body = featResponse.IsSuccess;
        }

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
