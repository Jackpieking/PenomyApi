using Microsoft.Extensions.Configuration;

namespace PenomyAPI.Infra.Configuration.Options;

public sealed class CorsOptions
{
    public string[] AllowOrigins { get; init; } = [];

    public string[] AllowMethods { get; init; } = [];

    public string[] AllowHeaders { get; init; } = [];

    public bool AllowCredentials { get; init; }

    public void Bind(IConfiguration configuration)
    {
        configuration
            .GetRequiredSection("Cors")
            .GetRequiredSection("Policy")
            .GetRequiredSection("Default")
            .Bind(this);
    }
}
