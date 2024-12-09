using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.FeatArt3.OtherHandlers.RemoveAllDeteledItems;

public class Art3RemoveAllDeletedItemsRequest
    : IFeatureRequest<Art3RemoveAllDeletedItemsResponse>
{
    public long CreatorId { get; set; }

    public ArtworkType ArtworkType { get; set; }
}
