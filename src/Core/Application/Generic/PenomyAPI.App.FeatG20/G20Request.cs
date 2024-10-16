using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG20;

public class G20Request : IFeatureRequest<G20Response>
{
    public long ArtworkId { get; set; }
}
