using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG1.OtherHandlers.CompleteRegistration;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.CompleteRegistration.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.CompleteRegistration.HttpResponseManager;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.CompleteRegistration.Middlewares.Validation;

internal sealed class G1CompleteRegistrationValidationPreProcessor
    : PreProcessor<G1CompleteRegistrationRequest, G1CompleteRegistrationStateBag>
{
    public override async Task PreProcessAsync(
        IPreProcessorContext<G1CompleteRegistrationRequest> context,
        G1CompleteRegistrationStateBag state,
        CancellationToken ct
    )
    {
        if (context.HasValidationFailures)
        {
            await context.HttpContext.Response.SendAsync(
                new G1CompleteRegistrationHttpResponse
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    AppCode =
                        $"G1CompleteRegistration.{G1CompleteRegistrationResponseStatusCode.INPUT_VALIDATION_FAIL}",
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
