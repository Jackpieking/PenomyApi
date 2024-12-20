using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG1.OtherHandlers.CompleteRegistration;

public sealed class G1CompleteRegistrationRequest : IFeatureRequest<G1CompleteRegistrationResponse>
{
    public string PreRegistrationToken { get; init; }

    public string ConfirmedNickName { get; init; }

    public string Password { get; init; }
}
