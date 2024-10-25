using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG56;

public class G56Request : IFeatureRequest<G56Response>
{
    public long CommentId { get; init; }

    public long UserId { get; init; }
}
