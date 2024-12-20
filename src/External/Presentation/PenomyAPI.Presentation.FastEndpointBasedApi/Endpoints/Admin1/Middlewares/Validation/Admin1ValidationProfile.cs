using FastEndpoints;
using FluentValidation;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Admin1.Middlewares.Validation;

public class Admin1ValidationProfile : Validator<Admin1HttpRequest>
{
    public Admin1ValidationProfile()
    {
        RuleFor(prop => prop.AdminApiKey).NotEmpty();

        RuleFor(prop => prop.CurrentCategoryId).Must(prop => prop >= 0);

        RuleFor(prop => prop.NumberOfCategoriesToTake).Must(prop => prop >= 0 && prop <= 20);
    }
}
