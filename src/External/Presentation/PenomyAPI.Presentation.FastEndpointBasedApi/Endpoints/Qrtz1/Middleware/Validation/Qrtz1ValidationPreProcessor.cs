using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Qrtz1.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Qrtz1.HttpRequest;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Qrtz1.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Qrtz1.Middleware.Validation;

public sealed class Qrtz1ValidationPreProcessor : PreProcessor<Qrtz1HttpRequest, Qrtz1StateBag>
{
    public override async Task PreProcessAsync(
        IPreProcessorContext<Qrtz1HttpRequest> context,
        Qrtz1StateBag state,
        CancellationToken ct
    )
    {
        if (context.HasValidationFailures)
        {
            await context.HttpContext.Response.SendAsync(
                new Qrtz1HttpResponse
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
