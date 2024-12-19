using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG5.OtherHandlers.CreatorProfileDetail;

public class G5CreatorProfileDetailRequest
    : IFeatureRequest<G5CreatorProfileDetailResponse>
{
    public long CreatorId { get; set; }
}
