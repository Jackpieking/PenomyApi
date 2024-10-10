using Microsoft.Extensions.Configuration;
using PenomyAPI.Infra.Configuration.Common;

namespace PenomyAPI.Infra.Configuration.Options;

public sealed class RedisOption : AppOptions
{
    public string ConnectionString { get; init; }

    public override void Bind(IConfiguration configuration)
    {
        configuration.GetRequiredSection("").Bind(this);

        Validate();
    }

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(value: ConnectionString))
        {
            throw new AppOptionBindingException(message: "RedisOption is not valid.");
        }
    }
}
