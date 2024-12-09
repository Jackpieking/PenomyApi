using PenomyAPI.App.Common;
using System.Collections.Generic;

namespace PenomyAPI.App.FeatArt3.OtherHandlers.RestoreDeletedItems;

public class Art3RestoreDeletedItemsRequest
    : IFeatureRequest<Art3RestoreDeletedItemResponse>
{
    public long CreatorId { get; set; }

    public IEnumerable<long> ArtworkIds { get; set; }
}
