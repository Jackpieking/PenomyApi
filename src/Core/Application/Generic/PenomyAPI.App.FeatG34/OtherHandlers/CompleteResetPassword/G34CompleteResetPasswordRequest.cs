using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG34.OtherHandlers.CompleteResetPassword;

public sealed class G34CompleteResetPasswordRequest
    : IFeatureRequest<G34CompleteResetPasswordResponse>
{
    public string Email { get; init; }

    public string ResetPasswordTokenId { get; init; }

    public string NewPassword { get; init; }
}
