using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.FeatArt1.OtherHandlers;

public sealed class Art1CountArtworkRequest
    : IFeatureRequest<Art1CountArtworkResponse>
{
    public long CreatorId { get; set; }

    public ArtworkType ArtworkType { get; set; }
}
