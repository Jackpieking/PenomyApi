using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG1;

public sealed class G1Response : IFeatureResponse
{
    public G1ResponseStatusCode StatusCode { get; init; }
}
