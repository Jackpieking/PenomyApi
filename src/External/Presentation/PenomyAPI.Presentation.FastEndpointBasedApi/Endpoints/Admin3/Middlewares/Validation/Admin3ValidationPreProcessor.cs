using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Admin3.Others;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Admin3.Middlewares.Validation;

public class Admin3ValidationPreProcessor : PreProcessor<Admin3HttpRequest, Admin3StateBag>
{
    public override async Task PreProcessAsync(
        IPreProcessorContext<Admin3HttpRequest> context,
        Admin3StateBag state,
        CancellationToken ct
    )
    {
        if (context.HasValidationFailures)
        {
            await context.HttpContext.Response.SendAsync(
                new Admin3HttpResponse
                {
                    Errors = context.ValidationFailures.Select(
                        failure => new Admin3HttpResponse.ErrorDto
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
