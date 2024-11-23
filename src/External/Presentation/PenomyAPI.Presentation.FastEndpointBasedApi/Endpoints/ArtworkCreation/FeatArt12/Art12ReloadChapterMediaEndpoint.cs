using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PenomyAPI.App.FeatArt12.OtherHandlers.ReloadChapterMedias;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt12.HttpResponseMappers;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt12.HttpResponses;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt12;

public sealed class Art12ReloadChapterMediaEndpoint
    : Endpoint<Art12ReloadChapterMediaRequest, Art12ReloadChapterMediaHttpResponse>
{
    public override void Configure()
    {
        Get("art12/chapter/reload");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<Art12ReloadChapterMediaRequest>>();
    }

    public override async Task<Art12ReloadChapterMediaHttpResponse> ExecuteAsync(
        Art12ReloadChapterMediaRequest request,
        CancellationToken ct)
    {
        var featureResponse = await FeatureExtensions
            .ExecuteAsync<Art12ReloadChapterMediaRequest, Art12ReloadChapterMediaResponse>(request, ct);

        var httpResponse = Art12ReloadChapterMediaHttpResponseMapper.Map(featureResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
