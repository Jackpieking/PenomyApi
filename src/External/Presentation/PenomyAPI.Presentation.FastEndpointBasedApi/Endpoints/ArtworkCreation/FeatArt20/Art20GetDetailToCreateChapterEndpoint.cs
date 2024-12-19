using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PenomyAPI.App.FeatArt20.OtherHandlers.GetDetailToCreateChapter;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt20.HttpResponses;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt20;

public sealed class Art20GetDetailToCreateChapterEndpoint
    : Endpoint<Art20GetDetailToCreateChapRequest, Art20GetDetailToCreateChapterHttpResponse>
{
    public override void Configure()
    {
        Get("art20/anime/{animeId}");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<Art20GetDetailToCreateChapRequest>>();
        PreProcessor<ArtworkCreationPreProcessor<Art20GetDetailToCreateChapRequest>>();
    }

    public override async Task<Art20GetDetailToCreateChapterHttpResponse> ExecuteAsync(
        Art20GetDetailToCreateChapRequest request,
        CancellationToken cancellationToken)
    {
        // Get the state bag contains creatorId extracted from the access-token.
        var stateBag = ProcessorState<StateBag>();

        request.SetCreatorId(stateBag.AppRequest.UserId);

        var featureResponse = await FeatureExtensions
            .ExecuteAsync<Art20GetDetailToCreateChapRequest, Art20GetDetailToCreateChapResponse>(
                request,
                cancellationToken);

        var httpResponse = Art20GetDetailToCreateChapterHttpResponse.MapFrom(featureResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, cancellationToken);

        return httpResponse;
    }
}
