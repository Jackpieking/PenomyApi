using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG34;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG34.InitResetPassword.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG34.InitResetPassword.HttpRequestManager;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG34.InitResetPassword.HttpResponseManager;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG34.InitResetPassword.Middlewares.Validation;

internal sealed class G34ValidationPreProcessor : PreProcessor<G34HttpRequest, G34StateBag>
{
    public override async Task PreProcessAsync(
        IPreProcessorContext<G34HttpRequest> context,
        G34StateBag state,
        CancellationToken ct
    )
    {
        if (context.HasValidationFailures)
        {
            await context.HttpContext.Response.SendAsync(
                new G34HttpResponse
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    AppCode = $"G34.{G34ResponseStatusCode.INPUT_VALIDATION_FAIL}",
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
