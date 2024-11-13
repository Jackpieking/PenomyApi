using FastEndpoints;
using PenomyAPI.App.FeatG2;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG2.HttpResponseMappers;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG2.HttpResponses;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1;

public sealed class G2Endpoint : Endpoint<G2Request, G2HttpResponse>
{
    public override void Configure()
    {
        Get("g2/top-recommended");

        AllowAnonymous();
    }

    public override async Task<G2HttpResponse> ExecuteAsync(G2Request request, CancellationToken ct)
    {
        var featResponse = await FeatureExtensions.ExecuteAsync<G2Request, G2Response>(
            request, ct);

        var httpResponse = G2HttpResponseMapper.Map(featResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
