using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG12;

public class G12Request : IFeatureRequest<G12Response>
{
    public long CategoryId { get; set; }
}
