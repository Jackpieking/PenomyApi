using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG34;

public sealed class G34Request : IFeatureRequest<G34Response>
{
    public string MailTemplate { get; init; }

    public string CurrentResetPasswordLink { get; init; }

    public string Email { get; init; }
}
