using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG14.OtherHandler;

public class G14GuestRequest : IFeatureRequest<G14GuestResponse>
{
    public long GuestId { get; set; }
    public int Limit { get; set; }
}
