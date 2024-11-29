using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.Extensions.Configuration;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Qrtz2.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Qrtz2.HttpRequest;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Qrtz2.Middleware.Authorization;

public sealed class Qrtz2AuthorizationPreProcessor : PreProcessor<Qrtz2HttpRequest, Qrtz2StateBag>
{
    private readonly IConfiguration _configuration;

    public Qrtz2AuthorizationPreProcessor(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public override async Task PreProcessAsync(
        IPreProcessorContext<Qrtz2HttpRequest> context,
        Qrtz2StateBag state,
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
