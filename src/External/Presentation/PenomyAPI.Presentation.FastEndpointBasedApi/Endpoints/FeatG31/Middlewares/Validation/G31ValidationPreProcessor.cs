using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG31;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG31.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG31.HttpResponseManager;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG31.Middlewares.Validation;

internal sealed class G31ValidationPreProcessor : PreProcessor<G31Request, G31StateBag>
{
    public override async Task PreProcessAsync(
        IPreProcessorContext<G31Request> context,
        G31StateBag state,
        CancellationToken ct
    )
    {
        if (context.HasValidationFailures)
        {
            await context.HttpContext.Response.SendAsync(
                new G31HttpResponse
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    AppCode = $"G31.{G31ResponseStatusCode.INPUT_VALIDATION_FAIL}",
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
