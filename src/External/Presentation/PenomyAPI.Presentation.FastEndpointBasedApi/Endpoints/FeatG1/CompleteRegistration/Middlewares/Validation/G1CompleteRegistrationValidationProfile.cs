using FastEndpoints;
using FluentValidation;
using PenomyAPI.App.FeatG1.OtherHandlers.CompleteRegistration;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.CompleteRegistration.Middlewares.Validation;

public sealed class G1CompleteRegistrationValidationProfile
    : Validator<G1CompleteRegistrationRequest>
{
    public G1CompleteRegistrationValidationProfile()
    {
        RuleFor(prop => prop.PreRegistrationToken).NotEmpty().MinimumLength(10);

        RuleFor(prop => prop.ConfirmedNickName)
            .NotEmpty()
            .MaximumLength(UserProfile.MetaData.NickNameLength)
            .MinimumLength(2);

        RuleFor(prop => prop.Password).NotEmpty().MinimumLength(8);
    }
}
