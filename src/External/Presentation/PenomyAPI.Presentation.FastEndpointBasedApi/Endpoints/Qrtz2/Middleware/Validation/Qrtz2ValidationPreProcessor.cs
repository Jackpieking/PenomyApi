using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Qrtz2.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Qrtz2.HttpRequest;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Qrtz2.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Qrtz2.Middleware.Validation;

public sealed class Qrtz2ValidationPreProcessor : PreProcessor<Qrtz2HttpRequest, Qrtz2StateBag>
{
    public override async Task PreProcessAsync(
        IPreProcessorContext<Qrtz2HttpRequest> context,
        Qrtz2StateBag state,
        CancellationToken ct
    )
    {
        if (context.HasValidationFailures)
        {
            await context.HttpContext.Response.SendAsync(
                new Qrtz2HttpResponse
                {
                    Errors = context.ValidationFailures.Select(failure => new
                    {
                        failure.PropertyName,
                        failure.ErrorMessage
                    })
                },
                StatusCodes.Status400BadRequest,
                cancellation: ct
            );
        }
    }
}
