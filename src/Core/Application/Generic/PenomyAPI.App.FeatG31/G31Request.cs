using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG31;

public sealed class G31Request : IFeatureRequest<G31Response>
{
    public string Email { get; init; }

    public string Password { get; init; }

    public bool RememberMe { get; init; }
}
