using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using PenomyAPI.Infra.Configuration.Common;

namespace PenomyAPI.Infra.Configuration.Options;

public sealed class JwtAuthenticationOptions : AppOptions
{
    public bool ValidateIssuer { get; set; }

    public bool ValidateAudience { get; set; }

    public bool ValidateLifetime { get; set; }

    public bool ValidateIssuerSigningKey { get; set; }

    public bool RequireExpirationTime { get; set; }

    public string ValidIssuer { get; set; }

    public string ValidAudience { get; set; }

    public string IssuerSigningKey { get; set; }

    public IEnumerable<string> ValidTypes { get; set; } = [];

    public override void Bind(IConfiguration configuration)
    {
        configuration.GetRequiredSection("Authentication").GetRequiredSection("Jwt").Bind(this);
    }
}
