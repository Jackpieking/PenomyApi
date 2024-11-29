using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM41;

public class SM41Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }
    public SM41ResponseStatusCode StatusCode { get; set; }
}
