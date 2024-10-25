namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG31.Common;

internal sealed class G31StateBag
{
    internal string CacheKey { get; set; }

    internal int CacheDurationInSeconds { get; } = 3 * 60;

    internal int CacheDurationForWrongPasswordStatusCodeInSeconds { get; } = 30;
}
