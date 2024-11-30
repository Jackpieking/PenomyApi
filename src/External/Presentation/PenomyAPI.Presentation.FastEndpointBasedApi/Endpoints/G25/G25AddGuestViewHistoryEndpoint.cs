using FastEndpoints;
using PenomyAPI.App.G25.OtherHandlers.AddGuestViewHistory;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25.HttpResponse;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25;

public class G25AddGuestViewHistoryEndpoint :
    Endpoint<G25AddGuestViewHistoryRequest, G25AddGuestViewHistoryHttpResponse>
{
    public override void Configure()
    {
        Post("g25/guest/add-history");

        AllowAnonymous();
    }

    public override async Task<G25AddGuestViewHistoryHttpResponse> ExecuteAsync(
        G25AddGuestViewHistoryRequest request,
        CancellationToken ct)
    {
        var featureResponse = await FeatureExtensions
            .ExecuteAsync<G25AddGuestViewHistoryRequest, G25AddGuestViewHistoryResponse>(request, ct);

        var httpResponse = G25AddGuestViewHistoryHttpResponse.MapFrom(featureResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
