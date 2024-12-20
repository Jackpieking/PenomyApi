using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PenomyAPI.App.FeatArt7.OtherHandlers.LoadComicDetail;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt7.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt7.HttpResponseManagers;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt7;

public sealed class Art7LoadComicDetailEndpoint
    : Endpoint<Art7LoadComicDetailRequest, Art7LoadComicDetailHttpResponse>
{
    public override void Configure()
    {
        Get("art7/comic/detail/{comicId:long}");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<Art7LoadComicDetailRequest>>();
        PreProcessor<ArtworkCreationPreProcessor<Art7LoadComicDetailRequest>>();
    }

    public override async Task<Art7LoadComicDetailHttpResponse> ExecuteAsync(
        Art7LoadComicDetailRequest request,
        CancellationToken ct
    )
    {
        // Get the state bag contains creatorId extracted from the access-token.
        var stateBag = ProcessorState<StateBag>();

        request.SetCreatorId(stateBag.AppRequest.UserId);

        var featResponse = await FeatureExtensions.ExecuteAsync<
            Art7LoadComicDetailRequest,
            Art7LoadComicDetailResponse
        >(request: request, ct: ct);

        var httpResponse = Art7LoadComicDetailHttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(featResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
