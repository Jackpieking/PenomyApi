using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Admin2.Others;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Admin2.Middlewares.Validation;

public class Admin2ValidationPreProcessor : PreProcessor<Admin2HttpRequest, Admin2StateBag>
{
    public override async Task PreProcessAsync(
        IPreProcessorContext<Admin2HttpRequest> context,
        Admin2StateBag state,
        CancellationToken ct
    )
    {
        if (context.HasValidationFailures)
        {
            await context.HttpContext.Response.SendAsync(
                new Admin2HttpResponse
                {
                    Errors = context.ValidationFailures.Select(
                        failure => new Admin2HttpResponse.ErrorDto
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
