using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM49;

public class SM49Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }
    public SM49ResponseStatusCode StatusCode { get; set; }
}
