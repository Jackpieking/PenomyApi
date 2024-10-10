using Microsoft.Extensions.Configuration;
using PenomyAPI.Infra.Configuration.Common;

namespace PenomyAPI.Infra.Configuration.Options;

public sealed class RedisSessionStorageOption : AppOptions
{
    public int IdleTimeoutInSecond { get; init; }

    public CookieOption Cookie { get; } = new();

    public sealed class CookieOption
    {
        public string Name { get; init; }

        public bool HttpOnly { get; init; }

        public bool IsEssential { get; init; }

        public int SecurePolicy { get; init; }

        public int SameSite { get; init; }
    }

    public override void Bind(IConfiguration configuration)
    {
        configuration.GetRequiredSection("").Bind(this);

        Validate();
    }

    private void Validate()
    {
        if (IdleTimeoutInSecond < 0 || string.IsNullOrWhiteSpace(value: Cookie.Name))
        {
            throw new AppOptionBindingException(message: "RedisSessionStorageOption is not valid.");
        }
    }
}
