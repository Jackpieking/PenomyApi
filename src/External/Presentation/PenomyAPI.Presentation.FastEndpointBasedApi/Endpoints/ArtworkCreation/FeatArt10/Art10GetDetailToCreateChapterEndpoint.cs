using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PenomyAPI.App.FeatArt10.OtherHandlers.GetDetailToCreateChapter;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.Common.Middlewares;
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

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<Art10GetDetailToCreateChapterRequest>>();
        PreProcessor<ArtworkCreationPreProcessor<Art10GetDetailToCreateChapterRequest>>();
    }

    public override async Task<Art10GetDetailToCreateChapterHttpResponse> ExecuteAsync(
        Art10GetDetailToCreateChapterRequest request,
        CancellationToken cancellationToken)
    {
        // Get the state bag contains creatorId extracted from the access-token.
        var stateBag = ProcessorState<StateBag>();

        request.SetCreatorId(stateBag.AppRequest.UserId);

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
