using FastEndpoints;
using FluentValidation;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Typs3.Middlewares.Validation;

public class Typs3ValidationProfile : Validator<Typs3HttpRequest>
{
    public Typs3ValidationProfile()
    {
        RuleFor(prop => prop.SearchText).NotEmpty();
    }
}
