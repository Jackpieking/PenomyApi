using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG1;

public sealed class G1Request : IFeatureRequest<G1Response>
{
    public string Email { get; init; }
}
