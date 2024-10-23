using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt1.OtherHandlers.OverviewStatistic;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt1.HttpResponse;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt1;

public class Art1OverviewStatisticEndpoint : EndpointWithoutRequest<Art1OverviewStatisticHttpResponse>
{
    public override void Configure()
    {
        Get("art1/overview-statistic");

        AllowAnonymous();
    }

    public override async Task<Art1OverviewStatisticHttpResponse> ExecuteAsync(CancellationToken ct)
    {
        var creatorId = 123456789012345678;
        var request = new Art1OverviewStatisticRequest
        {
            CreatorId = 123456789012345678
        };

        var featureResponse = await FeatureExtensions.ExecuteAsync<Art1OverviewStatisticRequest, Art1OverviewStatisticResponse>(
            request,
            ct);

        var httpResponse = new Art1OverviewStatisticHttpResponse
        {
            Body = featureResponse
        };

        await SendAsync(httpResponse, StatusCodes.Status200OK, ct);

        return httpResponse;
    }
}
