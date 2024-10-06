using PenomyAPI.App.Common;

namespace PenomyAPI.App.G43;

public class G43Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }
    public G43ResponseStatusCode StatusCode { get; set; }
}
