using FastEndpoints;
using FluentValidation;
using PenomyAPI.App.FeatG34.OtherHandlers.CompleteResetPassword;
using PenomyAPI.Domain.RelationalDb.Entities.UserIdentity;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG34.CompleteResetPassword.Middleware.Validation;

public sealed class G34CompleteResetPasswordValidationProfile
    : Validator<G34CompleteResetPasswordRequest>
{
    public G34CompleteResetPasswordValidationProfile()
    {
        RuleFor(prop => prop.ResetPasswordToken).NotEmpty().MinimumLength(10);

        RuleFor(prop => prop.NewPassword)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(User.MetaData.PasswordHashLength);
    }
}
