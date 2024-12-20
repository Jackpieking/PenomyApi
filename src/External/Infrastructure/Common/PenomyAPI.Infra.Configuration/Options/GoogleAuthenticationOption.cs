using Microsoft.Extensions.Configuration;
using PenomyAPI.Infra.Configuration.Common;

namespace PenomyAPI.Infra.Configuration.Options;

public sealed class GoogleAuthenticationOption : AppOptions
{
    public string ClientId { get; init; }

    public string ClientSecret { get; init; }

    public string CallBackPath { get; init; }

    public string ApiKey { get; init; }

    public string InitLoginPath { get; init; }

    public override void Bind(IConfiguration configuration)
    {
        configuration.GetRequiredSection("Authentication").GetRequiredSection("Google").Bind(this);
    }
}
