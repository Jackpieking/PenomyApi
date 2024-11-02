using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;

namespace PenomyAPI.App.FeatArt6;

public sealed class Art6Request : IFeatureRequest<Art6Response>
{
    public long ComicId { get; set; }

    public PublishStatus PublishStatus { get; set; }
}
