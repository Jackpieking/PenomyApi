using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM46;

public class SM46Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }
    public SM46ResponseStatusCode StatusCode { get; set; }
}
