using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG49.OtherHandlers;

public class G49StarRateRequest : IFeatureRequest<G49StarRateResponse>
{
    public long UserId { get; set; }
    public long ArtworkId { get; set; }
}
