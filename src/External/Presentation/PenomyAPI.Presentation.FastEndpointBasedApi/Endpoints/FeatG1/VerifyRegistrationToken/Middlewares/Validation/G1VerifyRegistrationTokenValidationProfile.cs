using FastEndpoints;
using FluentValidation;
using PenomyAPI.App.FeatG1.OtherHandlers.VerifyRegistrationToken;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.VerifyRegistrationToken.Middlewares.Validation;

public sealed class G1VerifyRegistrationTokenValidationProfile
    : Validator<G1VerifyRegistrationTokenRequest>
{
    public G1VerifyRegistrationTokenValidationProfile()
    {
        RuleFor(prop => prop.RegistrationToken).NotEmpty().MinimumLength(10);
    }
}
