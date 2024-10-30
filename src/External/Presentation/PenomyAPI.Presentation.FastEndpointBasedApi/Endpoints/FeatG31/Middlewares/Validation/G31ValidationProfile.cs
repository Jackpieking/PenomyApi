using FastEndpoints;
using FluentValidation;
using PenomyAPI.App.FeatG31;
using PenomyAPI.Domain.RelationalDb.Entities.UserIdentity;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG31.Middlewares.Validation;

public sealed class G31ValidationProfile : Validator<G31Request>
{
    public G31ValidationProfile()
    {
        RuleFor(prop => prop.Email)
            .NotEmpty()
            .EmailAddress()
            .MinimumLength(10)
            .MaximumLength(User.MetaData.EmailLength);

        RuleFor(prop => prop.Password).NotEmpty().MinimumLength(8);
    }
}
