using FastEndpoints;
using PenomyAPI.App.FeatArt5;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt5.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt5.HttpResponseMappers;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt5.HttpResponses;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt5;

public class Art5Endpoint : Endpoint<Art5RequestDto, Art5HttpResponse>
{
    public override void Configure()
    {
        Get("art5/comic/detail/{comicId}");

        AllowAnonymous();
    }

    public override async Task<Art5HttpResponse> ExecuteAsync(
        Art5RequestDto requestDto,
        CancellationToken ct)
    {
        var creatorId = 123456789012345678;
        var request = requestDto.MapToRequest(creatorId);

        var featureResponse = await FeatureExtensions.ExecuteAsync<Art5Request, Art5Response>(
            request,
            ct);

        var httpResponse = Art5HttpResponseMapper
            .Resolve(featureResponse.AppCode)
            .Invoke(featureResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
