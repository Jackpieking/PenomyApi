using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.Extensions.Configuration;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Admin2.Others;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Admin2.Middlewares.Authorization;

public class Admin2AuthorizationPreProcessor : PreProcessor<Admin2HttpRequest, Admin2StateBag>
{
    private readonly IConfiguration _configuration;

    public Admin2AuthorizationPreProcessor(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public override async Task PreProcessAsync(
        IPreProcessorContext<Admin2HttpRequest> context,
        Admin2StateBag state,
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
