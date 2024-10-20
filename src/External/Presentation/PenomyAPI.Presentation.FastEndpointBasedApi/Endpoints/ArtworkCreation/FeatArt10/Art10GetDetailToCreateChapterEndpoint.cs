using FastEndpoints;
using PenomyAPI.App.FeatArt10.OtherHandlers.GetDetailToCreateChapter;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt10.HttpResponseManagers;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt10.HttpResponses;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt10;

public sealed class Art10GetDetailToCreateChapterEndpoint
    : Endpoint<Art10GetDetailToCreateChapterRequest, Art10GetDetailToCreateChapterHttpResponse>
{
    public override void Configure()
    {
        Get("art10/comic/{comicId:long}");

        AllowAnonymous();
    }

    public override async Task<Art10GetDetailToCreateChapterHttpResponse> ExecuteAsync(
        Art10GetDetailToCreateChapterRequest request,
        CancellationToken cancellationToken)
    {
        var featureResponse = await FeatureExtensions
            .ExecuteAsync<Art10GetDetailToCreateChapterRequest, Art10GetDetailToCreateChapterResponse>(
                request,
                cancellationToken);

        var httpResponse = Art10GetDetailToCreateChapterHttpResponseManager
            .Resolve(featureResponse.AppCode)
            .Invoke(featureResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, cancellationToken);

        return httpResponse;
    }
}
