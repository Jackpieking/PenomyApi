using FastEndpoints;
using PenomyAPI.App.FeatG5.OtherHandlers.CreatorProfileDetail;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5_Support.HttpResponses;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5_Support;

public class G5CreatorDetailEndpoint : Endpoint<G5CreatorProfileDetailRequest, G5CreatorProfileHttpResponse>
{
    public override void Configure()
    {
        Get("g5/creator-profile/{creatorId}");

        AllowAnonymous();
    }

    public override async Task<G5CreatorProfileHttpResponse> ExecuteAsync(
        G5CreatorProfileDetailRequest request,
        CancellationToken ct)
    {
        var featResponse = await FeatureExtensions
            .ExecuteAsync<G5CreatorProfileDetailRequest, G5CreatorProfileDetailResponse>(request, ct);

        var httpResponse = G5CreatorProfileHttpResponse.MapFrom(featResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
