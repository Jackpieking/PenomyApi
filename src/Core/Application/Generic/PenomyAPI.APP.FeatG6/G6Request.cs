using PenomyAPI.App.Common;

namespace PenomyAPI.APP.FeatG6;

public class G6Request : IFeatureRequest<G6Response>
{
    public long ArtworkId { get; set; }

    public int TotalRecommendedArtworks { get; set; }
}
