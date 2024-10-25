using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatArt5;

public sealed class Art5Request : IFeatureRequest<Art5Response>
{
    public long CreatorId { get; set; }

    public long ComicId { get; set; }
}
