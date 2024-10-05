using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatArt7.OtherHandlers.LoadOrigin;

public sealed class Art7LoadOriginRequest : IFeatureRequest<Art7LoadOriginResponse>
{
    private static readonly Art7LoadOriginRequest _instance = new();

    private Art7LoadOriginRequest() { }

    public static Art7LoadOriginRequest Empty => _instance;
}
