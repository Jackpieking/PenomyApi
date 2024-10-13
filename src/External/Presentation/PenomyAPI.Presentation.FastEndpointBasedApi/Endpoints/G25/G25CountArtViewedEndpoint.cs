using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.G25;
using PenomyAPI.App.G25.OtherHandlers.NumberArtViewed;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt4.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG3.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25;

public class G25CountArtViewedEndpoint
    : Endpoint<G25CountArtViewedRequest, G25CountArtViewedHttpResponse>
{
    public override void Configure()
    {
        Get("/profile/user/history/count");

        AllowAnonymous();

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
        }

    public override async Task<G25CountArtViewedHttpResponse> ExecuteAsync(
        G25CountArtViewedRequest request,
        CancellationToken ct
    )
    {
        var featRequest = new G25CountArtViewedRequest
        {
            UserId = request.UserId,
            ArtworkType = request.ArtworkType,
        };

        var featResponse = await FeatureExtensions.ExecuteAsync<
            G25CountArtViewedRequest,
            G25CountArtViewedResponse
        >(featRequest, ct);

        G25CountArtViewedHttpResponse httpResponse = G25CountArtViewedResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(featRequest, featResponse);

        if (featResponse.IsSuccess)
        {
            httpResponse.Body = featResponse.ArtCount;

            return httpResponse;
        }

        return httpResponse;
    }
}
