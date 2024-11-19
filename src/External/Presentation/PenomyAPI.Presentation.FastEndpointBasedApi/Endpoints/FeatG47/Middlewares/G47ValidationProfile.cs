using FastEndpoints;
using FluentValidation;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG47.DTOs;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG47.Middlewares;

public class G47ValidationProfile : Validator<G47RequestDto>
{
    public G47ValidationProfile()
    {
        RuleFor(prop => prop.ArtworkId)
            .NotEmpty()
            .GreaterThan(0);
    }
}
