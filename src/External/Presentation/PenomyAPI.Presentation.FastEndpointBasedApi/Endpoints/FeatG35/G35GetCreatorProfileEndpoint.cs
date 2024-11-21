using FastEndpoints;
using PenomyAPI.App.FeatG35.OtherHandlers.GetCreatorProfile;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG35.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG35.HttpResponses;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG35;

public sealed class G35GetCreatorProfileEndpoint
    : Endpoint<G35GetCreatorProfileRequest, G35GetCreatorProfileHttpResponse>
{
    public override void Configure()
    {
        Get("g35/creator/profile");

        AllowAnonymous();
    }

    public override async Task<G35GetCreatorProfileHttpResponse> ExecuteAsync(
        G35GetCreatorProfileRequest request,
        CancellationToken ct)
    {
        var featureResponse = await FeatureExtensions
            .ExecuteAsync<G35GetCreatorProfileRequest, G35GetCreatorProfileResponse>(request, ct);

        var httpResponse = G35GetCreatorProfileHttpResponse.MapFrom(featureResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
