using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG1;

public sealed class G1Request : IFeatureRequest<G1Response>
{
    public string MailTemplate { get; init; }

    public string RedirectPageLink { get; init; }

    public string Email { get; init; }
}
