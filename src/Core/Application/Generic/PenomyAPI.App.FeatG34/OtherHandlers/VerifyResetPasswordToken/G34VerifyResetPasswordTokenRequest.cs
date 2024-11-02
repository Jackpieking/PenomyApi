using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG34.OtherHandlers.VerifyResetPasswordToken;

public sealed class G34VerifyResetPasswordTokenRequest
    : IFeatureRequest<G34VerifyResetPasswordTokenResponse>
{
    public string ResetPasswordToken { get; init; }
}
