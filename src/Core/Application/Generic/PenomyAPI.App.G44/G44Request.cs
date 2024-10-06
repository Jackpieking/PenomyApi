using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.G44;

public class G44Request : IFeatureRequest<G44Response>
{
    public long userId { get; set; }
    public long artworkId { get; set; }
    public ArtworkType ArtworkType { get; set; }
}
