using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.G25.OtherHandlers.GetGuestHistory;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25.HttpResponse;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25;

public class G25GetGuestViewHistoryEndpoint : Endpoint<G25GetGuestHitstoryRequest, G25GetGuestViewHistoryHttpResponse>
{
    public override void Configure()
    {
        Get("g25/guest/view-history");
        
        AllowAnonymous();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status404NotFound);
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status204NoContent);
        });
    }

    public override async Task<G25GetGuestViewHistoryHttpResponse> ExecuteAsync(
        G25GetGuestHitstoryRequest req,
        CancellationToken ct)
    {
        var featureResponse = await FeatureExtensions
            .ExecuteAsync<G25GetGuestHitstoryRequest, G25GetGuestHistoryResponse>(req, ct);

        var httpResponse = G25GetGuestViewHistoryHttpResponse.MapFrom(featureResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
