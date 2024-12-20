using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM30.SM30UnsendHandler;

public class SM30UnsendRequest : IFeatureRequest<SM30UnsendResponse>
{
    public long UserId { get; set; }
    public long FriendId { get; set; }
}
