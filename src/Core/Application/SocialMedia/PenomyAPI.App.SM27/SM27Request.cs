using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

namespace PenomyAPI.App.SM27;

public class SM27Request : IFeatureRequest<SM27Response>
{
    public long CommentId { get; init; }

    public long PostId { get; init; }
    
    private long _userId { get; set; }

    public void SetUserId(long userId) => _userId = userId;

    public long GetUserId() => _userId;
}
