using PenomyAPI.App.Common;

namespace PenomyAPI.App.G62;

public class G62Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }
    public G62ResponseStatusCode StatusCode { get; set; }
}
