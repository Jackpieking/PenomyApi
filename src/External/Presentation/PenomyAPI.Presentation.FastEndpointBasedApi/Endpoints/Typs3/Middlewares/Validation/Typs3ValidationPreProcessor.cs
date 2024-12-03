using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Typs3.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Typs3.Middlewares.Validation;

public class Typs3ValidationPreProcessor : PreProcessor<Typs3HttpRequest, Typs3StateBag>
{
    public override async Task PreProcessAsync(
        IPreProcessorContext<Typs3HttpRequest> context,
        Typs3StateBag state,
        CancellationToken ct
    )
    {
        if (context.HasValidationFailures)
        {
            await context.HttpContext.Response.SendAsync(
                new Typs3HttpResponse
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

            context.HttpContext.MarkResponseStart();
        }
    }
}
