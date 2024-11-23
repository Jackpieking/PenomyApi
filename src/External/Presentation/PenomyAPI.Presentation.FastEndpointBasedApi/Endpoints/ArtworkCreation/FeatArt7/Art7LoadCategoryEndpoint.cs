using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt7.OtherHandlers.LoadCategory;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.Common.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt7.HttpResponse;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt7;

public class Art7LoadCategoryEndpoint : EndpointWithoutRequest<Art7LoadCategoryHttpResponse>
{
    public override void Configure()
    {
        Get("art7/categories");

        AllowAnonymous();
    }

    public override async Task<Art7LoadCategoryHttpResponse> ExecuteAsync(CancellationToken ct)
    {
        var featResponse = await FeatureExtensions.ExecuteAsync<
            Art7LoadCategoryRequest,
            Art7LoadCategoryResponse
        >(Art7LoadCategoryRequest.Empty, ct);

        var httpResponse = new Art7LoadCategoryHttpResponse
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
