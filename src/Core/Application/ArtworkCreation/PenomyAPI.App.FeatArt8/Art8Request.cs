using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatArt8;

public sealed class Art8Request : IFeatureRequest<Art8Response>
{
    public long RemovedBy { get; set; }

    public long ArtworkId { get; set; }
}
