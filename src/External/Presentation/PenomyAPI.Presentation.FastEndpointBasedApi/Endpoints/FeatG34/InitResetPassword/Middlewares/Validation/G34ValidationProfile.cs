using FastEndpoints;
using FluentValidation;
using PenomyAPI.Domain.RelationalDb.Entities.UserIdentity;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG34.InitResetPassword.HttpRequestManager;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG34.InitResetPassword.Middlewares.Validation;

public sealed class G34ValidationProfile : Validator<G34HttpRequest>
{
    public G34ValidationProfile()
    {
        RuleFor(prop => prop.Email)
            .NotEmpty()
            .EmailAddress()
            .MinimumLength(10)
            .MaximumLength(User.MetaData.EmailLength);
    }
}
