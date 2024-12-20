using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Admin1.Others;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Admin1.Middlewares.Validation;

public class Admin1ValidationPreProcessor : PreProcessor<Admin1HttpRequest, Admin1StateBag>
{
    public override async Task PreProcessAsync(
        IPreProcessorContext<Admin1HttpRequest> context,
        Admin1StateBag state,
        CancellationToken ct
    )
    {
        if (context.HasValidationFailures)
        {
            await context.HttpContext.Response.SendAsync(
                new Admin1HttpResponse
                {
                    Errors = context.ValidationFailures.Select(
                        failure => new Admin1HttpResponse.ErrorDto
                        {
                            PropertyName = failure.PropertyName,
                            ErrorMessage = failure.ErrorMessage
                        }
                    )
                },
                StatusCodes.Status400BadRequest,
                cancellation: ct
            );
        }
    }
}
