using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM31;

public class SM31Response : IFeatureResponse
{
    private bool IsSuccess { get; set; }

    public SM31ResponseStatusCode StatusCode { get; set; }
}
