using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG34;

public sealed class G34Response : IFeatureResponse
{
    public G34ResponseStatusCode StatusCode { get; init; }
}
