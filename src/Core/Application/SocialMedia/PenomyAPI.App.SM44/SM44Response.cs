using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM44;

public class SM44Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }
    public SM44ResponseStatusCode StatusCode { get; set; }
}
