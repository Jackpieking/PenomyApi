using FastEndpoints;
using FluentValidation;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Qrtz2.HttpRequest;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Qrtz2.Middleware.Validation;

public sealed class Qrtz2ValidationProfile : Validator<Qrtz2HttpRequest>
{
    public Qrtz2ValidationProfile()
    {
        RuleFor(prop => prop.AdminApiKey).NotEmpty();
    }
}
