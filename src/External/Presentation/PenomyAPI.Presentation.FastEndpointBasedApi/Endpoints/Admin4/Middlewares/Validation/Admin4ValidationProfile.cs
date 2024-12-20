using FastEndpoints;
using FluentValidation;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Admin4.Middlewares.Validation;

public class Admin4ValidationProfile : Validator<Admin4HttpRequest>
{
    public Admin4ValidationProfile()
    {
        RuleFor(prop => prop.Email).NotEmpty().EmailAddress();

        RuleFor(prop => prop.Password).NotEmpty();
    }
}
