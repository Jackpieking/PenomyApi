using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG9.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG9.HttpResponses;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Helpers.Cache;

public interface ICommonCacheHandler
{
    ValueTask ClearG5MangaDetailCacheAsync(long mangaId, CancellationToken ct);

    Task<G5HttpResponse> GetOrSetG5MangaDetailCacheAsync(
        G5StateBag stateBag,
        G5RequestDto requestDto,
        CancellationToken ct
    );

    Task<G9HttpResponse> GetOrSetG9ChapterDetailCacheAsync(
        G9RequestDto requestDto,
        CancellationToken ct
    );

    ValueTask ClearG9ChapterDetailCacheAsync(long comicId, long chapterId, CancellationToken ct);
}
