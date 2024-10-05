using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG5;

public class G5Request : IFeatureRequest<G5Response>
{
    public long Id { get; set; }
}
