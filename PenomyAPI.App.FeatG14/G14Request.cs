using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG14;

public class G14Request : IFeatureRequest<G14Response>
{
    public long UserId { get; set; }
    public int Limit { get; set; }
}