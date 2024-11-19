using Microsoft.Extensions.Configuration;
using PenomyAPI.Infra.Configuration.Options.Base;

namespace PenomyAPI.Infra.Configuration.Options;

public sealed class PenomyNoReplyMailOptions : MailOptions
{
    public override void Bind(IConfiguration configuration)
    {
        configuration
            .GetRequiredSection("SmtpServer")
            .GetRequiredSection("PenomyGmail")
            .GetRequiredSection("NoReply")
            .Bind(this);
    }
}
