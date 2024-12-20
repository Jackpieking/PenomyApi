using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM32;

public class SM32Request : IFeatureRequest<SM32Response>
{
    public long UserId { get; set; }
    public long FriendId { get; set; }
}
