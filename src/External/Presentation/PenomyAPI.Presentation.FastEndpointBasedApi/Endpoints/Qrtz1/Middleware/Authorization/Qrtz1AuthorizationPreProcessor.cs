using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.Extensions.Configuration;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Qrtz1.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Qrtz1.HttpRequest;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Qrtz1.Middleware.Authorization;

public sealed class Qrtz1AuthorizationPreProcessor : PreProcessor<Qrtz1HttpRequest, Qrtz1StateBag>
{
    private readonly IConfiguration _configuration;

    public Qrtz1AuthorizationPreProcessor(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public override async Task PreProcessAsync(
        IPreProcessorContext<Qrtz1HttpRequest> context,
        Qrtz1StateBag state,
        CancellationToken ct
    )
    {
        var adminApiKeyFromConfiguration = _configuration
            .GetRequiredSection("Authentication")
            .GetValue<string>("AdminApiKey");

        if (!context.Request.AdminApiKey.Equals(adminApiKeyFromConfiguration))
        {
            await context.HttpContext.Response.SendUnauthorizedAsync();

            return;
        }
    }
}
