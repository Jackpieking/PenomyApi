using PenomyAPI.App.Common;

namespace PenomyAPI.App.G43;

public class G43Request : IFeatureRequest<G43Response>
{
    public long UserId { get; set; }
    public long ArtworkId { get; set; }
}
