using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG54;

public class G54Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }

    public G54ResponseStatusCode StatusCode { get; set; }
}
