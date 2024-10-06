using PenomyAPI.App.Common;

namespace PenomyAPI.App.G44;

public class G44Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }
    public G44ResponseStatusCode StatusCode { get; set; }
}
