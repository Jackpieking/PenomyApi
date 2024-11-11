using FastEndpoints;
using PenomyAPI.App.FeatG8.OtherHandlers;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG8.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG8.HttpResponseMappers;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG8;

public class G8GetPaginationOptionsEndpoint
    : Endpoint<G8GetPaginationOptionsRequest, G8GetPaginationOptionsHttpResponse>
{
    public override void Configure()
    {
        Get("g8/comic/pagination-options");

        AllowAnonymous();
    }

    public override async Task<G8GetPaginationOptionsHttpResponse> ExecuteAsync(
        G8GetPaginationOptionsRequest request,
        CancellationToken ct)
    {
        var featureResponse = await FeatureExtensions
            .ExecuteAsync<G8GetPaginationOptionsRequest, G8GetPaginationOptionsResponse>(
                request,
                ct);

        var httpResponse = G8GetPaginationOptionsHttpResponseMapper.Map(featureResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
