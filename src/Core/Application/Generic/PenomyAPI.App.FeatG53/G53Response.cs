using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG53;

public class G53Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }

    public G53ResponseStatusCode StatusCode { get; set; }
}
