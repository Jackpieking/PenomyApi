using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG33;

public sealed class G33Response : IFeatureResponse
{
    public G33ResponseStatusCode StatusCode { get; set; }
}
