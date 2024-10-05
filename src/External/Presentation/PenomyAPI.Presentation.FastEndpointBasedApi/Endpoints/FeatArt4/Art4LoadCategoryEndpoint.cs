using FastEndpoints;
using PenomyAPI.App.FeatArt4.OtherHandlers.LoadCategory;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatArt4.HttpResponse;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatArt4
{
    public class Art4LoadCategoryEndpoint : Endpoint<EmptyDto, Art4LoadCategoryHttpResponse>
    {
        public override void Configure()
        {
            Get("/art4/categories");

            AllowAnonymous();
        }

        public override async Task<Art4LoadCategoryHttpResponse> ExecuteAsync(
            EmptyDto req,
            CancellationToken ct
        )
        {
            var featRequest = Art4LoadCategoryRequest.Empty;

            var featResponse = await FeatureExtensions.ExecuteAsync<
                Art4LoadCategoryRequest,
                Art4LoadCategoryResponse
            >(featRequest, ct);

            return new Art4LoadCategoryHttpResponse { Categories = [], };
        }
    }
}
