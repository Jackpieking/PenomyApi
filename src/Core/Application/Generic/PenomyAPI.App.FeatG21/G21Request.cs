using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG21;

public class G21Request : IFeatureRequest<G21Response>
{
    public long ChapterId { get; set; }

    public long UserId { get; set; }
}
