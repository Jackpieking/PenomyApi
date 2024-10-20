using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG1.OtherHandlers.VerifyRegistrationToken;

public sealed class G1VerifyRegistrationTokenResponse : IFeatureResponse
{
    public bool IsValid { get; set; }

    public string Email { get; set; }
}
