using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM7.OtherHandlers.CountGroups;

public class SM7CountGroupsResponse : IFeatureResponse
{
    public int TotalGroups { get; set; }
}
