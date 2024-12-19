using FastEndpoints;
using PenomyAPI.App.FeatG19.OtherHandlers.GetChapterList;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG19.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG19.HttpResponses;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG19;

public class G19GetChapterListEndpoint : Endpoint<G19GetChapterListRequestDto, G19GetChapterListHttpResponse>
{
    public override void Configure()
    {
        Get("g19/anime/chapter-list");

        AllowAnonymous();
    }

    public override async Task<G19GetChapterListHttpResponse> ExecuteAsync(
        G19GetChapterListRequestDto requestDto,
        CancellationToken ct
    )
    {
        var request = new G19GetChapterListRequest { AnimeId = requestDto.AnimeId, UserId = 1, };

        var featResponse = await FeatureExtensions.ExecuteAsync<
            G19GetChapterListRequest,
            G19GetChapterListResponse
        >(request, ct);

        var httpResponse = G19GetChapterListHttpResponse.MapFrom(featResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
