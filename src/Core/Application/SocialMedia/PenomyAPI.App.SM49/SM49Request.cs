using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM49;

public class SM49Request : IFeatureRequest<SM49Response>
{
    public long UserId { get; set; }
    public long FriendId { get; set; }
}
