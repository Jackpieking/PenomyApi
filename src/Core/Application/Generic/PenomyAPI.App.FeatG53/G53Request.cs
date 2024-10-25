using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG53;

public class G53Request : IFeatureRequest<G53Response>
{
    public string CommentId { get; init; }

    public string NewComment { get; init; }
}
