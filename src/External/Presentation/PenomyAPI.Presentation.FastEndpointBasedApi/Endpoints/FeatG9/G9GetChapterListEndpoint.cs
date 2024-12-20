using FastEndpoints;
using PenomyAPI.App.FeatG9.OtherHandlers.GetChapterList;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG9.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG9.HttpResponses;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG9;

public class G9GetChapterListEndpoint : Endpoint<G9GetChapterListRequestDto, G9GetChapterListHttpResponse>
{
    public override void Configure()
    {
        Get("g9/chapter-list");

        AllowAnonymous();
    }

    public override async Task<G9GetChapterListHttpResponse> ExecuteAsync(
        G9GetChapterListRequestDto requestDto,
        CancellationToken ct
    )
    {
        var request = new G9GetChapterListRequest { ComicId = requestDto.ComicId, UserId = 1, };

        var featResponse = await FeatureExtensions.ExecuteAsync<
            G9GetChapterListRequest,
            G9GetChapterListResponse
        >(request, ct);

        var httpResponse = G9GetChapterListHttpResponse.MapFrom(featResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
