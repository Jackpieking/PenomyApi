using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Helpers.Cache;

public interface ICommonCacheHandler
{
    ValueTask ClearG5MangaDetailCacheAsync(int mangaId, CancellationToken ct);

    Task<G5HttpResponse> GetOrSetG5MangaDetailCacheAsync(
        G5StateBag stateBag,
        G5RequestDto requestDto,
        CancellationToken ct
    );
}
