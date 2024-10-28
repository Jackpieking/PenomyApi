using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG34.OtherHandlers.VerifyResetPasswordToken;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG34.VerifyResetPasswordToken.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG34.VerifyResetPasswordToken.HttpResponseManager;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG34.VerifyResetPasswordToken.Middlewares.Validation;

internal sealed class G34VerifyResetPasswordTokenValidationPreProcessor
    : PreProcessor<G34VerifyResetPasswordTokenRequest, G34VerifyResetPasswordTokenStateBag>
{
    public override async Task PreProcessAsync(
        IPreProcessorContext<G34VerifyResetPasswordTokenRequest> context,
        G34VerifyResetPasswordTokenStateBag state,
        CancellationToken ct
    )
    {
        if (context.HasValidationFailures)
        {
            await context.HttpContext.Response.SendAsync(
                new G34VerifyResetPasswordTokenHttpResponse
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    AppCode =
                        $"G34VerifyResetPasswordToken.{G34VerifyResetPasswordTokenResponseStatusCode.INPUT_VALIDATION_FAIL}",
                    Errors = context.ValidationFailures.Select(failure => new
                    {
                        failure.PropertyName,
                        failure.ErrorMessage
                    })
                },
                StatusCodes.Status400BadRequest,
                cancellation: ct
            );

            context.HttpContext.MarkResponseStart();
        }
    }
}
