using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG34.OtherHandlers.CompleteResetPassword;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG34.CompleteResetPassword.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG34.CompleteResetPassword.HttpResponseManager;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG34.CompleteResetPassword.Middleware.Validation;

public class G34CompleteResetPasswordValidationPreProcessor
    : PreProcessor<G34CompleteResetPasswordRequest, G34CompleteResetPasswordStateBag>
{
    public override async Task PreProcessAsync(
        IPreProcessorContext<G34CompleteResetPasswordRequest> context,
        G34CompleteResetPasswordStateBag state,
        CancellationToken ct
    )
    {
        if (context.HasValidationFailures)
        {
            await context.HttpContext.Response.SendAsync(
                new G34CompleteResetPasswordHttpResponse
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    AppCode =
                        $"G34CompleteResetPassword.{G34CompleteResetPasswordResponseStatusCode.INPUT_VALIDATION_FAIL}",
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
