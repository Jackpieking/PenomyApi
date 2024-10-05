using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatArt7.OtherHandlers.LoadPublicLevel;

public sealed class Art7LoadPublicLevelRequest : IFeatureRequest<Art7LoadPublicLevelResponse>
{
    private static readonly Art7LoadPublicLevelRequest _instance = new();

    private Art7LoadPublicLevelRequest() { }

    public static Art7LoadPublicLevelRequest Empty => _instance;
}
