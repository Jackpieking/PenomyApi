using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM30;

public class SM30Response : IFeatureResponse
{
    private bool IsSuccess { get; set; }

    public SM30ResponseStatusCode StatusCode { get; set; }
}
