using Microsoft.Extensions.Configuration;
using PenomyAPI.Infra.Configuration.Common;

namespace PenomyAPI.Infra.Configuration.Options;

public sealed class GoogleSignInOption : AppOptions
{
    public InitOption Init { get; init; } = new();

    public VerifyOption Verify { get; init; } = new();

    public sealed class InitOption
    {
        public string ResponseRedirectUrl { get; init; }

        public string CookieDomain { get; init; }
    }

    public sealed class VerifyOption
    {
        public string ResponseRedirectBaseUrl { get; init; }
    }

    public override void Bind(IConfiguration configuration)
    {
        configuration.GetRequiredSection("Others").GetRequiredSection("GoogleSignIn").Bind(this);
    }
}
