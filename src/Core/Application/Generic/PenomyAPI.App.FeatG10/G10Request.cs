using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG10;

public class G10Request : IFeatureRequest<G10Response>
{
    public long ArtworkId { get; set; }
}
