using PenomyAPI.App.Common;
using System.Collections.Generic;

namespace PenomyAPI.App.FeatArt3.OtherHandlers.RemoveDeletedItems;

public class Art3RemoveDeletedItemsRequest
    : IFeatureRequest<Art3RemoveDeletedItemsResponse>
{
    public long CreatorId { get; set; }

    public IEnumerable<long> DeletedArtworkIds { get; set; }
}
