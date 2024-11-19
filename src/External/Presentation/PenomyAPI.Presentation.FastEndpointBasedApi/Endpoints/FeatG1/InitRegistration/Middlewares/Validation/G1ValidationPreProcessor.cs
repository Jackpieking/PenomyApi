using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG1;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.InitRegistration.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.InitRegistration.HttpRequest;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.InitRegistration.HttpResponseManager;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.InitRegistration.Middlewares.Validation;

internal sealed class G1ValidationPreProcessor : PreProcessor<G1HttpRequest, G1StateBag>
{
    public override async Task PreProcessAsync(
        IPreProcessorContext<G1HttpRequest> context,
        G1StateBag state,
        CancellationToken ct
    )
    {
        if (context.HasValidationFailures)
        {
            await context.HttpContext.Response.SendAsync(
                new G1HttpResponse
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    AppCode = $"G1.{G1ResponseStatusCode.INPUT_VALIDATION_FAIL}",
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
