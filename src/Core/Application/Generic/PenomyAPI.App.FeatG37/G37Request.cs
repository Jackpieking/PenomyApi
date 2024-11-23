using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG37;

public sealed class G37Request : IFeatureRequest<G37Response>
{
    public long UserId { get; set; }
}
