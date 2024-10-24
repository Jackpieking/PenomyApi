namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.VerifyRegistrationToken.Common;

internal sealed class G1VerifyRegistrationTokenStateBag
{
    internal string CacheKey { get; set; }

    internal int CacheDurationInSeconds { get; } = 180;
}
