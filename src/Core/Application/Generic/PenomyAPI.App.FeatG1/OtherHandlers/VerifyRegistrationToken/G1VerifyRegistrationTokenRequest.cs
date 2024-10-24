using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG1.OtherHandlers.VerifyRegistrationToken;

public sealed class G1VerifyRegistrationTokenRequest
    : IFeatureRequest<G1VerifyRegistrationTokenResponse>
{
    public string RegistrationToken { get; init; }
}
