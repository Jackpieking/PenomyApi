using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.G25;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25.HttpResponse;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25
{
    public class G25SaveArtViewHistEndpoint : Endpoint<G25SaveArtViewHistRequest, G25SaveArtViewHistHttpResponse>
    {
        public override void Configure()
        {
            Get("/profile/user/history/save");

            AllowAnonymous();

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
                    example: new() { AppCode = G25SaveArtViewHistResponseStatusCode.SUCCESS.ToString() }
                );
            });
        }

        public override async Task<G25SaveArtViewHistHttpResponse> ExecuteAsync(G25SaveArtViewHistRequest request, CancellationToken ct)
        {
            var featRequest = new G25SaveArtViewHistRequest
            {
                UserId = request.UserId,
                ArtworkId = request.ArtworkId,
                ChapterId = request.ChapterId,
                ArtworkType = request.ArtworkType
            };

            var featResponse = await FeatureExtensions.ExecuteAsync<G25SaveArtViewHistRequest, G25SaveArtViewHistResponse>(
                featRequest,
                ct
            );

            G25SaveArtViewHistHttpResponse httpResponse = G25SaveArtViewHistResponseManager
                .Resolve(featResponse.StatusCode)
                .Invoke(featRequest, featResponse);

            if (featResponse.IsSuccess)
            {

                httpResponse.Body = featResponse.IsSuccess;

                return httpResponse;
            }

            return httpResponse;
        }
    }
}
