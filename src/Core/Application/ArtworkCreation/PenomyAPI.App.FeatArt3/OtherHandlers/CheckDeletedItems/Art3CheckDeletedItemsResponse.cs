using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Models.ArtworkCreation.FeatArt3;

namespace PenomyAPI.App.FeatArt3.OtherHandlers.CheckDeletedItems;

public class Art3CheckDeletedItemsResponse : IFeatureResponse
{
    public Art3CheckDeletedItemReadModel Result { get; set; }
}
