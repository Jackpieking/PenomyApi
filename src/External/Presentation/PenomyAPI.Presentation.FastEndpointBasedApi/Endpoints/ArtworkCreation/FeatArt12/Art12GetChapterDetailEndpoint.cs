using FastEndpoints;
using PenomyAPI.App.FeatArt12.OtherHandlers.GetChapterDetail;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt12.DTOs.GetChapterDetail;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt12.HttpResponseMappers;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt12.HttpResponses;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt12;

public class Art12GetChapterDetailEndpoint
    : Endpoint<Art12GetChapterDetailRequestDto, Art12GetChapterDetailHttpResponse>
{
    public override void Configure()
    {
        Get("art12/chapter/{chapterId:long}");

        AllowAnonymous();
    }

    public override async Task<Art12GetChapterDetailHttpResponse> ExecuteAsync(
        Art12GetChapterDetailRequestDto requestDto,
        CancellationToken ct)
    {
        var creatorId = 123456789012345678;
        var request = requestDto.MapTo(creatorId);

        var featureResponse = await FeatureExtensions.ExecuteAsync<Art12GetChapterDetailRequest, Art12GetChapterDetailResponse>(
            request,
            ct);

        var httpResponse = Art12GetChapterDetailHttpResponseMapper.Map(featureResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
