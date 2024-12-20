using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM50;

public class SM50Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }
    public SM50ResponseStatusCode StatusCode { get; set; }
}
