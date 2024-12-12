using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.G25.OtherHandlers.InitGuestHistory;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25.HttpResponse;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25;

public class G25InitGuestViewHistoryEndpoint : EndpointWithoutRequest<G25InitGuestViewHistoryHttpResponse>
{
    public override void Configure()
    {
        Post("g25/guest/init-history");

        AllowAnonymous();
    }

    public override async Task<G25InitGuestViewHistoryHttpResponse> ExecuteAsync(CancellationToken ct)
    {
        var request = G25InitGuestHistoryRequest.Instance;

        var featureResponse = await FeatureExtensions
            .ExecuteAsync<G25InitGuestHistoryRequest, G25InitGuestHistoryResponse>(request, ct);

        var httpResponse = new G25InitGuestViewHistoryHttpResponse
        {
            HttpCode = StatusCodes.Status201Created,
            Body = new()
            {
                GuestId = featureResponse.GuestId.ToString(),
                LastActiveAt = featureResponse.LastActiveAt,
            }
        };

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
