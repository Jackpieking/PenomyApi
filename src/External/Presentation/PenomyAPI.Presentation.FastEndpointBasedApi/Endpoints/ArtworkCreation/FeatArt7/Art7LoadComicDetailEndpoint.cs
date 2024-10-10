using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using PenomyAPI.App.FeatArt7.OtherHandlers.LoadComicDetail;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt7.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt7.HttpResponseManagers;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt7
{
    public sealed class Art7LoadComicDetailEndpoint
        : Endpoint<Art7LoadComicDetailRequest, Art7LoadComicDetailHttpResponse>
    {
        public override void Configure()
        {
            Get("art7/detail/{comicId:long}");

            AllowAnonymous();
        }

        public override async Task<Art7LoadComicDetailHttpResponse> ExecuteAsync(
            Art7LoadComicDetailRequest request,
            CancellationToken ct
        )
        {
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
}
