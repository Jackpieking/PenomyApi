using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.FeatG28.PageCount;

public class G28PageCountRequest : IFeatureRequest<G28PageCountResponse>
{
    public long CreatorId { get; set; }

    public ArtworkType ArtworkType { get; init; }
}
