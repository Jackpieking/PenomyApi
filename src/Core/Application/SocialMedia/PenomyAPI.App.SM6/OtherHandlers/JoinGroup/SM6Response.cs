using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM6.OtherHandlers.JoinGroup;

public class SM6JoinGroupResponse : IFeatureResponse
{
    public bool IsSuccess { get; set; }
    public SM6ResponseStatusCode StatusCode { get; set; }
}
