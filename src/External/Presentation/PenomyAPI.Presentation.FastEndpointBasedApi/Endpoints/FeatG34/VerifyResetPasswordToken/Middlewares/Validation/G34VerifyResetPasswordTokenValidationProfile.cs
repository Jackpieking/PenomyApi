using FastEndpoints;
using FluentValidation;
using PenomyAPI.App.FeatG34.OtherHandlers.VerifyResetPasswordToken;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG34.VerifyResetPasswordToken.Middlewares.Validation;

public sealed class G34VerifyResetPasswordTokenValidationProfile
    : Validator<G34VerifyResetPasswordTokenRequest>
{
    public G34VerifyResetPasswordTokenValidationProfile()
    {
        RuleFor(prop => prop.ResetPasswordToken).NotEmpty().MinimumLength(10);
    }
}
