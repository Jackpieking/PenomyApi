using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PenomyAPI.App.FeatArt17.OtherHandlers.GetDetail;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt17.HttpResponses;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt17;

public class Art17GetAnimeDetailEndpoint
    : Endpoint<Art17GetAnimeDetailRequest, Art17GetAnimeDetailHttpResponse>
{
    public override void Configure()
    {
        Get("art17/anime/detail/{artworkId}");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<Art17GetAnimeDetailRequest>>();
        PreProcessor<ArtworkCreationPreProcessor<Art17GetAnimeDetailRequest>>();
    }

    public override async Task<Art17GetAnimeDetailHttpResponse> ExecuteAsync(
        Art17GetAnimeDetailRequest request,
        CancellationToken ct
    )
    {
        // Get the state bag contains creatorId extracted from the access-token.
        var stateBag = ProcessorState<StateBag>();

        request.SetCreatorId(stateBag.AppRequest.UserId);

        var featResponse = await FeatureExtensions.ExecuteAsync<
            Art17GetAnimeDetailRequest,
            Art17GetAnimeDetailResponse
        >(request: request, ct: ct);

        var httpResponse = Art17GetAnimeDetailHttpResponse.MapFrom(featResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
