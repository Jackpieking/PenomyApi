using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.Extensions.Configuration;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Admin1.Others;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Admin1.Middlewares.Authorization;

public class Admin1AuthorizationPreProcessor : PreProcessor<Admin1HttpRequest, Admin1StateBag>
{
    private readonly IConfiguration _configuration;

    public Admin1AuthorizationPreProcessor(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public override async Task PreProcessAsync(
        IPreProcessorContext<Admin1HttpRequest> context,
        Admin1StateBag state,
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
