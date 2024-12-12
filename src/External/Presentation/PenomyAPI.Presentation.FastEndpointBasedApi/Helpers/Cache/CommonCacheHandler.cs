using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.FeatG5;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5.HttpResponse;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Serialization;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Helpers.Cache;

public class CommonCacheHandler : ICommonCacheHandler
{
    private readonly Lazy<IFusionCache> _fusionCache;
    private readonly Lazy<IFusionCacheSerializer> _serializer;
    #region G5
    private const string G5_ARTWORK_ID = "G5_input_ArtworkId_";
    #endregion

    public CommonCacheHandler(
        Lazy<IFusionCache> fusionCache,
        Lazy<IFusionCacheSerializer> serializer
    )
    {
        _fusionCache = fusionCache;
        _serializer = serializer;
    }

    public ValueTask ClearG5MangaDetailCacheAsync(int mangaId, CancellationToken ct)
    {
        return _fusionCache.Value.RemoveAsync($"{G5_ARTWORK_ID}{mangaId}", token: ct);
    }

    public async Task<G5HttpResponse> GetOrSetG5MangaDetailCacheAsync(
        G5StateBag stateBag,
        G5RequestDto requestDto,
        CancellationToken ct
    )
    {
        var result = await _fusionCache.Value.GetOrSetAsync(
            $"{G5_ARTWORK_ID}{requestDto.ArtworkId}",
            async ct =>
            {
                var httpResponse = await GetHttpResponseAsync(stateBag, requestDto, ct);

                return await _serializer.Value.SerializeAsync(httpResponse, ct);
            },
            options: new()
            {
                Duration = TimeSpan.FromSeconds(120),
                IsFailSafeEnabled = true,
                FailSafeMaxDuration = TimeSpan.FromSeconds(240),
                FailSafeThrottleDuration = TimeSpan.FromSeconds(30),
                FactorySoftTimeout = TimeSpan.FromSeconds(100),
                FactoryHardTimeout = TimeSpan.FromSeconds(500),
                AllowTimedOutFactoryBackgroundCompletion = true
            },
            ct
        );

        return await _serializer.Value.DeserializeAsync<G5HttpResponse>(result, ct);
    }

    private static async Task<G5HttpResponse> GetHttpResponseAsync(
        G5StateBag stateBag,
        G5RequestDto requestDto,
        CancellationToken ct
    )
    {
        var g5Req = new G5Request
        {
            ForSignedInUser = stateBag.IsAuthenticated,
            UserId = stateBag.UserId,
            GuestId = requestDto.GuestId,
            ComicId = requestDto.ArtworkId,
        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<G5Request, G5Response>(g5Req, ct);

        var httpResponse = G5HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(g5Req, featResponse);

        return httpResponse;
    }
}
