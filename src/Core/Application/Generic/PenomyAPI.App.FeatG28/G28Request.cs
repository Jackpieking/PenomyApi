using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.FeatG28;

public class G28Request : IFeatureRequest<G28Response>
{
    public long CreatorId { get; set; }

    public ArtworkType ArtworkType { get; init; }

    public int PageNumber { get; init; }
}
