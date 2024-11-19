using FastEndpoints;
using FluentValidation;
using PenomyAPI.App.FeatG31A;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG31A.Middlewares.Validation;

public sealed class G31AValidationProfile : Validator<G31ARequest>
{
    public G31AValidationProfile()
    {
        RuleFor(prop => prop.RefreshToken).NotEmpty();
    }
}
