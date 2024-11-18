using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM7.OtherHandlers.CountGroups;

public class SM7CountGroupsRequest : IFeatureRequest<SM7CountGroupsResponse>
{
    public long UserId { get; set; }
}
