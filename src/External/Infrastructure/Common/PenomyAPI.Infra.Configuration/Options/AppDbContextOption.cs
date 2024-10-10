using Microsoft.Extensions.Configuration;
using PenomyAPI.Infra.Configuration.Common;

namespace PenomyAPI.Infra.Configuration.Options;

public sealed class AppDbContextOption : AppOptions
{
    public string ConnectionString { get; init; }

    public int CommandTimeOut { get; init; }

    public int EnableRetryOnFailure { get; init; }

    public bool EnableSensitiveDataLogging { get; init; }

    public bool EnableDetailedErrors { get; init; }

    public bool EnableThreadSafetyChecks { get; init; }

    public bool EnableServiceProviderCaching { get; init; }

    public override void Bind(IConfiguration configuration)
    {
        configuration.GetRequiredSection("").Bind(this);

        Validate();
    }

    private void Validate()
    {
        if (
            string.IsNullOrWhiteSpace(value: ConnectionString)
            || CommandTimeOut < 5
            || EnableRetryOnFailure < 3
            || EnableServiceProviderCaching != true
        )
        {
            throw new AppOptionBindingException(message: "AppDbContextOption is not valid.");
        }
    }
}
