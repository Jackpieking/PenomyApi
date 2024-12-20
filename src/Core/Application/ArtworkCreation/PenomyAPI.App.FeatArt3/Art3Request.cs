using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.FeatArt3;

public class Art3Request : IFeatureRequest<Art3Response>
{
    public long CreatorId { get; set; }

    public ArtworkType ArtworkType { get; set; }
}
