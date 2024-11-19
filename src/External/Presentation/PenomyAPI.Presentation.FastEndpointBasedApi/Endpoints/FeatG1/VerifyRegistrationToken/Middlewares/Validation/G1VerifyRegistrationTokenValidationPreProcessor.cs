using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG1.OtherHandlers.VerifyRegistrationToken;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.VerifyRegistrationToken.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.VerifyRegistrationToken.HttpResponseManager;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.VerifyRegistrationToken.Middlewares.Validation;

internal sealed class G1VerifyRegistrationTokenValidationPreProcessor
    : PreProcessor<G1VerifyRegistrationTokenRequest, G1VerifyRegistrationTokenStateBag>
{
    public override async Task PreProcessAsync(
        IPreProcessorContext<G1VerifyRegistrationTokenRequest> context,
        G1VerifyRegistrationTokenStateBag state,
        CancellationToken ct
    )
    {
        if (context.HasValidationFailures)
        {
            await context.HttpContext.Response.SendAsync(
                new G1VerifyRegistrationTokenHttpResponse
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    AppCode =
                        $"G1VerifyRegistrationToken.{G1VerifyRegistrationTokenResponseStatusCode.INPUT_VALIDATION_FAIL}",
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
