using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG59;

public class G59Request : IFeatureRequest<G59Response>
{
    public long ParentCommentId { get; set; }

    public long UserId { get; set; }
}
