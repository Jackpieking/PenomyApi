using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG54;

public class G54Request : IFeatureRequest<G54Response>
{
    public Guid ArtworkCommentId { get; set; }
}
