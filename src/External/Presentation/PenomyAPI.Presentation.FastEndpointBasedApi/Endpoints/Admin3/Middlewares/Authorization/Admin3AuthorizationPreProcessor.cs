using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.Extensions.Configuration;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Admin3.Others;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Admin3.Middlewares.Authorization;

public sealed class Admin3AuthorizationPreProcessor
    : PreProcessor<Admin3HttpRequest, Admin3StateBag>
{
    private readonly IConfiguration _configuration;

    public Admin3AuthorizationPreProcessor(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public override async Task PreProcessAsync(
        IPreProcessorContext<Admin3HttpRequest> context,
        Admin3StateBag state,
        CancellationToken ct
    )
    {
        if (context.HttpContext.ResponseStarted())
        {
            return;
        }

        var adminApiKeyFromConfiguration = _configuration
            .GetRequiredSection("Authentication")
            .GetValue<string>("AdminApiKey");

        if (!context.Request.AdminApiKey.Equals(adminApiKeyFromConfiguration))
        {
            await context.HttpContext.Response.SendUnauthorizedAsync(ct);

            return;
        }
    }
}
