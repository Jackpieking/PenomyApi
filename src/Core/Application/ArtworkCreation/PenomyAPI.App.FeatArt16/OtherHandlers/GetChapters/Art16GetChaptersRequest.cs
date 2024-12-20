using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;

namespace PenomyAPI.App.FeatArt16.OtherHandlers.GetChapters;

public class Art16GetChaptersRequest : IFeatureRequest<Art16GetChaptersResponse>
{
    public long AnimeId { get; set; }

    public PublishStatus PublishStatus { get; set; }
}
