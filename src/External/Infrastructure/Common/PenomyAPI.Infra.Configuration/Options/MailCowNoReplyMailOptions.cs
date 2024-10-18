using Microsoft.Extensions.Configuration;
using PenomyAPI.Infra.Configuration.Common;

namespace PenomyAPI.Infra.Configuration.Options;

public sealed class MailCowNoReplyMailOptions : AppOptions
{
    public string DisplayName { get; init; }

    public string Host { get; init; }

    public string Password { get; init; }

    public int Port { get; init; }

    public string Mail { get; init; }

    public override void Bind(IConfiguration configuration)
    {
        configuration
            .GetRequiredSection("SmtpServer")
            .GetRequiredSection("MailCow")
            .GetRequiredSection("NoReply")
            .Bind(this);
    }
}
