using FastEndpoints;
using FluentValidation;
using PenomyAPI.Domain.RelationalDb.Entities.UserIdentity;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.InitRegistration.HttpRequest;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.InitRegistration.Middlewares.Validation;

public sealed class G1ValidationProfile : Validator<G1HttpRequest>
{
    public G1ValidationProfile()
    {
        RuleFor(prop => prop.Email)
            .NotEmpty()
            .EmailAddress()
            .MinimumLength(10)
            .MaximumLength(User.MetaData.EmailLength);
    }
}
