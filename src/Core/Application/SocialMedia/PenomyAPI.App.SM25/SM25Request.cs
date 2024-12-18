using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM25;

public class SM25Request : IFeatureRequest<SM25Response>
{
    public long CommentId { get; init; }

    public string NewComment { get; init; }

    private long _userId { get; set; }

    public void SetUserId(long userId) => _userId = userId;

    public long GetUserId() => _userId;
}
