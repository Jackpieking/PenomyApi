using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using PenomyAPI.App.FeatArt1;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt1.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt1.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt1.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt1.HttpResponseManagers;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt1;

public sealed class Art1Endpoint : Endpoint<Art1RequestDto, Art1HttpResponse>
{
    public override void Configure()
    {
        Get("art1/artworks");

        AllowAnonymous();
    }

    public override async Task<Art1HttpResponse> ExecuteAsync(
        Art1RequestDto requestDto,
        CancellationToken ct
    )
    {
        const long creatorId = 123456789012345678;
        var featRequest = requestDto.MapToRequest(
            creatorId: creatorId,
            pageSize: Art1PaginationOptions.DEFAULT_PAGE_SIZE
        );

        var featResponse = await FeatureExtensions.ExecuteAsync<Art1Request, Art1Response>(
            featRequest,
            ct
        );

        var httpResponse = Art1HttpResponseManager
            .Resolve(featResponse.AppCode)
            .Invoke(featResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
