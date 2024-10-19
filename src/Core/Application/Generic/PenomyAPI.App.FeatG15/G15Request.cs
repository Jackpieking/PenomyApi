using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG15;

public class G15Request : IFeatureRequest<G15Response>
{
    public long Id { get; set; }
}
