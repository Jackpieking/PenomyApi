using FastEndpoints;
using FluentValidation;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Admin3.Middlewares.Validation;

public class Admin3ValidationProfile : Validator<Admin3HttpRequest>
{
    public Admin3ValidationProfile()
    {
        RuleFor(prop => prop.AdminApiKey).NotEmpty();

        RuleFor(prop => prop.Category.Id).NotEmpty();

        RuleFor(prop => prop.Category.Name).NotEmpty();

        RuleFor(prop => prop.Category.Description).NotEmpty();
    }
}
