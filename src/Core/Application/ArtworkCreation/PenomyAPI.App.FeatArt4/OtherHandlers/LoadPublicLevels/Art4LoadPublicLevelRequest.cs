using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatArt4.OtherHandlers.LoadPublicLevels;

public sealed class Art4LoadPublicLevelRequest : IFeatureRequest<Art4LoadPublicLevelResponse>
{
    private static readonly Art4LoadPublicLevelRequest _instance = new();

    private Art4LoadPublicLevelRequest() { }

    public static Art4LoadPublicLevelRequest Empty => _instance;
}
