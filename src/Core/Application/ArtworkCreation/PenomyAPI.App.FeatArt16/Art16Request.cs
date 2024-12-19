using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatArt16;

public class Art16Request : IFeatureRequest<Art16Response>
{
    public long CreatorId { get; set; }

    public long ArtworkId { get; set; }
}
