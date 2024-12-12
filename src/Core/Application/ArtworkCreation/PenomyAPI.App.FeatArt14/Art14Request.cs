using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatArt14;

public sealed class Art14Request : IFeatureRequest<Art14Response>
{
    public long CreatorId { get; set; }

    public long ArtworkId { get; set; }

    public long ChapterId { get; set; }
}
