using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.FeatArt3.OtherHandlers.RestoreAllDeletedItems;

public class Art3RestoreAllDeletedItemsRequest
    : IFeatureRequest<Art3RestoreAllDeletedItemsResponse>
{
    public long CreatorId { get; set; }

    public ArtworkType ArtworkType { get; set; }
}
