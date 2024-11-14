using PenomyAPI.App.Common;

namespace PenomyAPI.App.G44;

public class G44Request : IFeatureRequest<G44Response>
{
    public long UserId { get; set; }
    public long ArtworkId { get; set; }
}
