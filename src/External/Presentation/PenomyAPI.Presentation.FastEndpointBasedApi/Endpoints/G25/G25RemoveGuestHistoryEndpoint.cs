using FastEndpoints;
using PenomyAPI.App.G25.OtherHandlers.RemoveGuestHistoryItem;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25.HttpResponse;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25;

public sealed class G25RemoveGuestHistoryEndpoint
    : Endpoint<G25RemoveGuestHistoryItemRequest, G25RemoveGuestHistoryHttpReponse>
{
    public override void Configure()
    {
        Post("g25/guest/remove-history");

        AllowAnonymous();
    }

    public override async Task<G25RemoveGuestHistoryHttpReponse> ExecuteAsync(
        G25RemoveGuestHistoryItemRequest request,
        CancellationToken ct)
    {
        var featResponse = await FeatureExtensions
            .ExecuteAsync<G25RemoveGuestHistoryItemRequest, G25RemoveGuestHistoryItemReponse>(request, ct);

        var httpResponse = G25RemoveGuestHistoryHttpReponse.MapFrom(featResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
