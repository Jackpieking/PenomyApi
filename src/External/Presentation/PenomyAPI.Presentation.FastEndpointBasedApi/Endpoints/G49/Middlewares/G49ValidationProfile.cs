using FastEndpoints;
using FluentValidation;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G49.DTOs;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G49.Middlewares;

public class G49ValidationProfile : Validator<G49RequestDto>
{
    public G49ValidationProfile()
    {
        RuleFor(prop => prop.ArtworkId)
            .NotEmpty()
            .GreaterThan(0);
        RuleFor(prop => prop.StarRate)
            .LessThanOrEqualTo((byte)5)
            .GreaterThanOrEqualTo((byte)0);
    }
}
