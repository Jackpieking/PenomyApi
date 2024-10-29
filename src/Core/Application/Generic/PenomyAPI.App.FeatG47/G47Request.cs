using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG47;

public class G47Request : IFeatureRequest<G47Response>
{
    public long UserId { get; set; }
    public long ArtworkId { get; set; }
}
