namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.InitRegistration.Common;

internal sealed class G1StateBag
{
    internal string CacheKey { get; set; }

    internal int CacheDurationInSeconds { get; } = 180;
}
