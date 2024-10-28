using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.FeatG58;

public class G58Request : IFeatureRequest<G58Response>
{
    public ArtworkComment ReplyComment { get; init; }

    public long ParentCommentId { get; init; }
}
