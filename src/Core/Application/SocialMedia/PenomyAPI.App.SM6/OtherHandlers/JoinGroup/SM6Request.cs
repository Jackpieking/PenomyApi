using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM6.OtherHandlers.JoinGroup;

public class SM6JoinGroupRequest : IFeatureRequest<SM6JoinGroupResponse>
{
    public long UserId { get; set; }
    public long GroupId { get; set; }
}
