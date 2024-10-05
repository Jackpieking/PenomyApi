using Microsoft.Extensions.Configuration;

namespace PenomyAPI.Infra.Configuration.Options;

public sealed class PenomyCorsOptions
{
    private const string RootSectionName = "CORS";

    public string[] AllowOrigins { get; init; }

    public void Bind(IConfiguration configuration)
    {
        configuration.GetRequiredSection(RootSectionName).Bind(this);
    }
}
