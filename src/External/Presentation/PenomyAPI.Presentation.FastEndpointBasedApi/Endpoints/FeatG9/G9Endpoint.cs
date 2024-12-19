using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG9.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG9.HttpResponses;
using PenomyAPI.Presentation.FastEndpointBasedApi.Helpers.Cache;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG9;

public sealed class G9Endpoint : Endpoint<G9RequestDto, G9HttpResponse>
{
    private readonly ICommonCacheHandler _cacheHandler;

    public G9Endpoint(ICommonCacheHandler cacheHandler)
    {
        _cacheHandler = cacheHandler;
    }

    public override void Configure()
    {
        Get("g9/comic/{comicId:long}/chapter/{chapterId:long}");

        AllowAnonymous();
    }

    public override async Task<G9HttpResponse> ExecuteAsync(
        G9RequestDto requestDto,
        CancellationToken cancellationToken
    )
    {
        var httpResponse = await _cacheHandler.GetOrSetG9ChapterDetailCacheAsync(
            requestDto,
            cancellationToken
        );

        await SendAsync(httpResponse, httpResponse.HttpCode, cancellationToken);

        return httpResponse;
    }
}
