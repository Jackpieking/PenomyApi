using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM31;

public class SM31Request : IFeatureRequest<SM31Response>
{
    public long UserId { get; set; }
    public long FriendId { get; set; }
}
