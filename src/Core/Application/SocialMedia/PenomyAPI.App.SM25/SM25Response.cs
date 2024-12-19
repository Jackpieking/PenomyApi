using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM25;

public class SM25Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }

    public SM25ResponseStatusCode StatusCode { get; set; }
}
