using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG34.OtherHandlers.VerifyResetPasswordToken;

public sealed class G34VerifyResetPasswordTokenResponse : IFeatureResponse
{
    public G34VerifyResetPasswordTokenResponseStatusCode StatusCode { get; init; }

    public string ResetPasswordToken { get; init; }
}
