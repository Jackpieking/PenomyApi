using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Admin4.Others;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Admin4.Middlewares.Validation;

public class Admin4ValidationPreProcessor : PreProcessor<Admin4HttpRequest, Admin4StateBag>
{
    public override async Task PreProcessAsync(
        IPreProcessorContext<Admin4HttpRequest> context,
        Admin4StateBag state,
        CancellationToken ct
    )
    {
        if (context.HasValidationFailures)
        {
            await context.HttpContext.Response.SendAsync(
                new Admin4HttpResponse
                {
                    Errors = context.ValidationFailures.Select(
                        failure => new Admin4HttpResponse.ErrorDto
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
