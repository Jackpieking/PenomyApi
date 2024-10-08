using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt4.OtherHandlers.LoadCategory;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.Common.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt4.HttpResponse;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt4;

public class Art4LoadCategoryEndpoint : EndpointWithoutRequest<Art4LoadCategoryHttpResponse>
{
    public override void Configure()
    {
        Get("art4/categories");

        AllowAnonymous();
    }

    public override async Task<Art4LoadCategoryHttpResponse> ExecuteAsync(CancellationToken ct)
    {
        var featResponse = await FeatureExtensions.ExecuteAsync<
            Art4LoadCategoryRequest,
            Art4LoadCategoryResponse
        >(Art4LoadCategoryRequest.Empty, ct);

        var httpResponse = new Art4LoadCategoryHttpResponse
        {
            HttpCode = StatusCodes.Status200OK,
            Body = featResponse.Categories.Select(category => new CategoryDto
            {
                Id = category.Id.ToString(),
                Label = category.Name,
            })
        };

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
