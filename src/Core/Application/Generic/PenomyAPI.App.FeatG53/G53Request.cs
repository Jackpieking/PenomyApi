using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.FeatG53;

public class G53Request : IFeatureRequest<G53Response>
{
    public long CommentId { get; init; }

    public string NewComment {  get; init; }

}
