using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG1.OtherHandlers.CompleteRegistration;

public sealed class G1CompleteRegistrationResponse : IFeatureResponse
{
    public string NewUserEmail { get; init; }

    public G1CompleteRegistrationResponseStatusCode StatusCode { get; init; }
}
