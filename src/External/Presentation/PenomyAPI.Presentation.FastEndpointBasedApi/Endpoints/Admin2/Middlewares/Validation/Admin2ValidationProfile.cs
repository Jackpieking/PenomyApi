using FastEndpoints;
using FluentValidation;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Admin2.Middlewares.Validation;

public class Admin2ValidationProfile : Validator<Admin2HttpRequest>
{
    public Admin2ValidationProfile()
    {
        RuleFor(prop => prop.AdminApiKey).NotEmpty();

        RuleFor(prop => prop.CategoryId).Must(prop => prop >= 0);
    }
}
