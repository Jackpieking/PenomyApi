using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM6;

public class SM6Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }
    public SM6ResponseStatusCode StatusCode { get; set; }
}
