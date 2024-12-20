using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM45;

public class SM45Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }
    public SM45ResponseStatusCode StatusCode { get; set; }
}
