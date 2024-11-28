using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM40;

public class SM40Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }
    public SM40ResponseStatusCode StatusCode { get; set; }
}
