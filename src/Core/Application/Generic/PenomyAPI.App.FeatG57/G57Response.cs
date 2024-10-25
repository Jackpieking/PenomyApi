using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG57;

public class G57Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }

    public G57ResponseStatusCode StatusCode { get; set; }
}
