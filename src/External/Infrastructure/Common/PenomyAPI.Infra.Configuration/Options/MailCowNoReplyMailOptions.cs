using Microsoft.Extensions.Configuration;
using PenomyAPI.Infra.Configuration.Options.Base;

namespace PenomyAPI.Infra.Configuration.Options;

public sealed class MailCowNoReplyMailOptions : MailOptions
{
    public override void Bind(IConfiguration configuration)
    {
        configuration
            .GetRequiredSection("SmtpServer")
            .GetRequiredSection("MailCow")
            .GetRequiredSection("NoReply")
            .Bind(this);
    }
}
