namespace PenomyAPI.App.Common.Caching;

/// <summary>
///     Wrapper around value that is used for caching.
/// </summary>
/// <typeparam name="TSource">
///     Type of source
/// </typeparam>
public sealed class AppCacheModel<TSource>
{
    /// <summary>
    ///     Value as TSource type.
    /// </summary>
    public TSource Value { get; init; }

    /// <summary>
    ///     Represent the state cannot found
    ///     the cached value by key.
    /// </summary>
    public static readonly AppCacheModel<TSource> NotFound;
}
