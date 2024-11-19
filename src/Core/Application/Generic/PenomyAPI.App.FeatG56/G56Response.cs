using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG56;

public class G56Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }

    public G56ResponseStatusCode StatusCode { get; set; }
}
