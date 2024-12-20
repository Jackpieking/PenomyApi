using FastEndpoints;
using FluentValidation;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Qrtz1.HttpRequest;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Qrtz1.Middleware.Validation;

public sealed class Qrtz1ValidationProfile : Validator<Qrtz1HttpRequest>
{
    public Qrtz1ValidationProfile()
    {
        RuleFor(prop => prop.AdminApiKey).NotEmpty();
    }
}
