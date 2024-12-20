using FastEndpoints;
using PenomyAPI.App.FeatG5.OtherHandlers.GetArtworkMetaData;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5_Support.HttpResponses;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5_Support;

public class G5ArtworkMetaDataEndpoint : Endpoint<G5ArtworkMetaDataRequest, G5ArtworkMetaDataHttpResponse>
{
    public override void Configure()
    {
        Get("g5/artwork/metadata/{artworkId}");

        AllowAnonymous();
    }

    public override async Task<G5ArtworkMetaDataHttpResponse> ExecuteAsync(
        G5ArtworkMetaDataRequest request,
        CancellationToken ct)
    {
        var featResponse = await FeatureExtensions
            .ExecuteAsync<G5ArtworkMetaDataRequest, G5ArtworkMetaDataResponse>(request, ct);

        var httpResponse = G5ArtworkMetaDataHttpResponse.MapFrom(featResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
