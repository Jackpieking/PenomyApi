using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatArt4.OtherHandlers.LoadOrigin;

public sealed class Art4LoadOriginRequest : IFeatureRequest<Art4LoadOriginResponse>
{
    private static readonly Art4LoadOriginRequest _instance = new();

    private Art4LoadOriginRequest() { }

    public static Art4LoadOriginRequest Empty => _instance;
}
