using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG35;

public sealed class G35Request : IFeatureRequest<G35Response>
{
    public long UserId { get; set; }
}
