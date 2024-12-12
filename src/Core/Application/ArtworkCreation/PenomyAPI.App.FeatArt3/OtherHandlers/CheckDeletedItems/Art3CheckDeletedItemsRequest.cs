using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatArt3.OtherHandlers.CheckDeletedItems;

public sealed class Art3CheckDeletedItemsRequest
    : IFeatureRequest<Art3CheckDeletedItemsResponse>
{
    public long CreatorId { get; set; }
}
