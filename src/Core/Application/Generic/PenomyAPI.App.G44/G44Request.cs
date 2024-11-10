using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.G44;

public class G44Request : IFeatureRequest<G44Response>
{
    public long UserId { get; set; }
    public long ArtworkId { get; set; }
    public ArtworkType ArtworkType { get; set; }
}
