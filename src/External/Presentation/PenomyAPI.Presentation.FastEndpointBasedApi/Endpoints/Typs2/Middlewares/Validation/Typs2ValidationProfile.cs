using FastEndpoints;
using FluentValidation;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Typs2.Middlewares.Validation;

public class Typs2ValidationProfile : Validator<Typs2HttpRequest>
{
    public Typs2ValidationProfile()
    {
        RuleFor(prop => prop.SearchText).NotEmpty();
    }
}
