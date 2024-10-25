using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG57;

public class G57Request : IFeatureRequest<G57Response>
{
    public long CommentId { get; init; }

    public long UserId { get; init; }

}
