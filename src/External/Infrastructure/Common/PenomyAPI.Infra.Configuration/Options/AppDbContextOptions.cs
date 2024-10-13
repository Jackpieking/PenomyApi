namespace PenomyAPI.Infra.Configuration.Options;

public sealed class AppDbContextOptions
{
    public string ConnectionString { get; init; }

    public int CommandTimeOutInSecond { get; init; }

    public int EnableRetryOnFailure { get; init; }

    public bool EnableSensitiveDataLogging { get; init; }

    public bool EnableDetailedErrors { get; init; }

    public bool EnableThreadSafetyChecks { get; init; }

    public bool EnableServiceProviderCaching { get; init; }
}
