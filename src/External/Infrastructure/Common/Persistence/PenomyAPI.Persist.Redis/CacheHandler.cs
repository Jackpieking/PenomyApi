using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common.Caching;
using PenomyAPI.App.Common.Serializer;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Serialization;

namespace PenomyAPI.Persist.Redis;

/// <summary>
///     Implementation of cache handler using redis as storage.
/// </summary>
public sealed class CacheHandler : ICacheHandler
{
    private readonly Lazy<IFusionCache> _fusionCache;
    private readonly Lazy<IFusionCacheSerializer> _serializer;

    public CacheHandler(Lazy<IFusionCache> fusionCache, Lazy<IFusionCacheSerializer> serializer)
    {
        _fusionCache = fusionCache;
        _serializer = serializer;
    }

    public async Task<AppCacheModel<TSource>> GetAsync<TSource>(
        string key,
        CancellationToken cancellationToken
    )
        where TSource : IAppProtobufObject
    {
        var cachedValue = await _fusionCache.Value.GetOrDefaultAsync<byte[]>(
            key,
            token: cancellationToken
        );

        if (Equals(cachedValue, default))
        {
            return AppCacheModel<TSource>.NotFound;
        }

        return new()
        {
            Value = await _serializer.Value.DeserializeAsync<TSource>(
                cachedValue,
                cancellationToken
            )
        };
    }

    public async Task<TSource> GetOrSetAsync<TSource>(
        string key,
        Func<CancellationToken, Task<TSource>> value,
        AppCacheOption cacheOption,
        CancellationToken cancellationToken
    )
        where TSource : IAppProtobufObject
    {
        var result = await _fusionCache.Value.GetOrSetAsync(
            key,
            async ct => await _serializer.Value.SerializeAsync(await value(ct), ct),
            options: new()
            {
                Duration = TimeSpan.FromSeconds(cacheOption.DurationInSeconds),
                IsFailSafeEnabled = cacheOption.IsFailSafeEnabled,
                FailSafeMaxDuration = TimeSpan.FromSeconds(
                    cacheOption.FailSafeMaxDurationInSeconds
                ),
                FailSafeThrottleDuration = TimeSpan.FromSeconds(
                    cacheOption.FailSafeThrottleDurationInSeconds
                ),
                FactorySoftTimeout = TimeSpan.FromSeconds(cacheOption.FactorySoftTimeoutInSeconds),
                FactoryHardTimeout = TimeSpan.FromSeconds(cacheOption.FactoryHardTimeoutInSeconds),
                AllowTimedOutFactoryBackgroundCompletion =
                    cacheOption.AllowTimedOutFactoryBackgroundCompletion,
            },
            cancellationToken
        );

        return await _serializer.Value.DeserializeAsync<TSource>(result, cancellationToken);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken)
    {
        await _fusionCache.Value.RemoveAsync(key, token: cancellationToken);
    }

    public async Task SetAsync<TSource>(
        string key,
        TSource value,
        AppCacheOption cacheOption,
        CancellationToken cancellationToken
    )
        where TSource : IAppProtobufObject
    {
        await _fusionCache.Value.SetAsync(
            key,
            await _serializer.Value.SerializeAsync(value, cancellationToken),
            new()
            {
                Duration = TimeSpan.FromSeconds(cacheOption.DurationInSeconds),
                IsFailSafeEnabled = cacheOption.IsFailSafeEnabled,
                FailSafeMaxDuration = TimeSpan.FromSeconds(
                    cacheOption.FailSafeMaxDurationInSeconds
                ),
                FailSafeThrottleDuration = TimeSpan.FromSeconds(
                    cacheOption.FailSafeThrottleDurationInSeconds
                ),
                FactorySoftTimeout = TimeSpan.FromSeconds(cacheOption.FactorySoftTimeoutInSeconds),
                FactoryHardTimeout = TimeSpan.FromSeconds(cacheOption.FactoryHardTimeoutInSeconds),
                AllowTimedOutFactoryBackgroundCompletion =
                    cacheOption.AllowTimedOutFactoryBackgroundCompletion,
            },
            token: cancellationToken
        );
    }
}
