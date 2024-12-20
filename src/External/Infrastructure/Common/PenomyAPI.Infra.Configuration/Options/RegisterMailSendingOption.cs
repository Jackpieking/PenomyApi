using Microsoft.Extensions.Configuration;
using PenomyAPI.Infra.Configuration.Common;

namespace PenomyAPI.Infra.Configuration.Options;

public sealed class RegisterMailSendingOption : AppOptions
{
    public string MailTemplateRelativePath { get; init; }

    public string VerifyRegisterLink { get; init; }

    public override void Bind(IConfiguration configuration)
    {
        configuration
            .GetRequiredSection("Others")
            .GetRequiredSection("RegisterMailSending")
            .Bind(this);
    }
}
