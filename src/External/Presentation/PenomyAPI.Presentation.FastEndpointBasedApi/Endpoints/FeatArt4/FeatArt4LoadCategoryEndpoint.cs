using FastEndpoints;
using PenomyAPI.App.FeatArt4.OtherHandlers.LoadCategory;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatArt4.HttpResponse;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatArt4
{
    public class FeatArt4LoadCategoryEndpoint : Endpoint<EmptyDto, FeatG3LoadCategoryHttpResponse>
    {
        public override void Configure()
        {
            Get("/art4/categories");
            
            AllowAnonymous();
        }

        public override async Task<FeatG3LoadCategoryHttpResponse> ExecuteAsync(EmptyDto req, CancellationToken ct)
        {
            var featRequest = Art4LoadCategoryRequest.Empty;

            var featResponse = await FeatureExtensions.ExecuteAsync<Art4LoadCategoryRequest, Art4LoadCategoryResponse>(featRequest, ct);

            return new FeatG3LoadCategoryHttpResponse
            {
                Categories = [],
            };
        }
    }
}
