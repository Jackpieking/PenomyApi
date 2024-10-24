namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.CompleteRegistration.Common;

internal sealed class G1CompleteRegistrationStateBag
{
    internal string CacheKey { get; set; }

    internal int CacheDurationInSeconds { get; } = 180;
}
