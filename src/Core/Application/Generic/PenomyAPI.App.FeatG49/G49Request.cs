using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG49;

public class G49Request : IFeatureRequest<G49Response>
{
    public long UserId { get; set; }
    public long ArtworkId { get; set; }
    public byte StarRate { get; set; }
}
