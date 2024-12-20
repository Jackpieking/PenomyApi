using PenomyAPI.App.Common;

namespace PenomyAPI.App.G61.OtherHandlers.CheckHasFollow;

public sealed class G61CheckHasFollowRequest : IFeatureRequest<G61CheckHasFollowResponse>
{
    public long UserId { get; set; }

    public long CreatorId { get; set; }
}
