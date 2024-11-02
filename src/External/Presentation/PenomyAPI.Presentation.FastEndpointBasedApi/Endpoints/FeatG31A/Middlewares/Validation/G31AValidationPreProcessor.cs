using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG31A;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG31A.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG31A.HttpResponseManager;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG31A.Middlewares.Validation;

internal sealed class G31AValidationPreProcessor : PreProcessor<G31ARequest, G31AStateBag>
{
    public override async Task PreProcessAsync(
        IPreProcessorContext<G31ARequest> context,
        G31AStateBag state,
        CancellationToken ct
    )
    {
        if (context.HasValidationFailures)
        {
            await context.HttpContext.Response.SendAsync(
                new G31AHttpResponse
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    AppCode = $"G31A.{G31AResponseStatusCode.INPUT_VALIDATION_FAIL}",
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
