using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.G25.OtherHandlers.CountArtwork;

public class G25CountArtworkRequest : IFeatureRequest<G25CountArtworkResponse>
{
    public long UserId { get; set; }
    public ArtworkType ArtworkType { get; set; }
}
