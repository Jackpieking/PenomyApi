using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Typs2.Others;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Typs2.Middlewares.Validation;

public class Typs2ValidationPreProcessor : PreProcessor<Typs2HttpRequest, Typs2StateBag>
{
    public override async Task PreProcessAsync(
        IPreProcessorContext<Typs2HttpRequest> context,
        Typs2StateBag state,
        CancellationToken ct
    )
    {
        if (context.HasValidationFailures)
        {
            await context.HttpContext.Response.SendAsync(
                new Typs2HttpResponse
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
