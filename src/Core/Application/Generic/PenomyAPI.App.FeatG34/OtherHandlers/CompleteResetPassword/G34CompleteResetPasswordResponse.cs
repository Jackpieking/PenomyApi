using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG34.OtherHandlers.CompleteResetPassword;

public sealed class G34CompleteResetPasswordResponse : IFeatureResponse
{
    public G34CompleteResetPasswordResponseStatusCode StatusCode { get; init; }
}
