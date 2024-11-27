using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM30;

public class SM30Request : IFeatureRequest<SM30Response>
{
    public long UserId { get; set; }
    public long FriendId { get; set; }
}
