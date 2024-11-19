using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common.Serializer;

namespace PenomyAPI.App.Common.Caching;

/// <summary>
///     Represent interface of caching handler.
/// </summary>
public interface ICacheHandler
{
    /// <summary>
    ///     Get the value by key.
    /// </summary>
    /// <typeparam name="TSource">
    ///     Type of value.
    /// </typeparam>
    /// <param name="key">
    ///     Key to find the value.
    /// </param>
    /// <param name="cancellationToken">
    ///     A token that is used to notify the system
    ///     to cancel the current operation when user stop
    ///     the request.
    /// </param>
    /// <returns>
    ///     The task containing the cache model.
    /// </returns>
    Task<AppCacheModel<TSource>> GetAsync<TSource>(string key, CancellationToken cancellationToken)
        where TSource : IAppProtobufObject;

    /// <summary>
    ///     Set the new key-value pair.
    /// </summary>
    /// <typeparam name="TSource">
    ///     Type of value.
    /// </typeparam>
    /// <param name="key">
    ///     Key to find the value.
    /// </param>
    /// <param name="value">
    ///     Value for the key.
    /// </param>
    /// <param name="distributedCacheEntryOptions">
    ///     Option for distributed cache.
    /// </param>
    /// <param name="cancellationToken">
    ///     A token that is used to notify the system
    ///     to cancel the current operation when user stop
    ///     the request.
    /// </param>
    /// <returns>
    ///     True if success. Otherwise, false.
    /// </returns>
    Task SetAsync<TSource>(
        string key,
        TSource value,
        AppCacheOption cacheOption,
        CancellationToken cancellationToken
    )
        where TSource : IAppProtobufObject;

    /// <summary>
    ///     Remove the value by given key.
    /// </summary>
    /// <param name="key">
    ///     Key to find the value.
    /// </param>
    /// <returns>
    ///     True if success. Otherwise, false.
    /// </returns>
    /// <param name="cancellationToken">
    ///     A token that is used to notify the system
    ///     to cancel the current operation when user stop
    ///     the request.
    /// </param>
    Task RemoveAsync(string key, CancellationToken cancellationToken);
}
