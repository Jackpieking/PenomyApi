using Microsoft.Extensions.Configuration;
using PenomyAPI.Infra.Configuration.Common;

namespace PenomyAPI.Infra.Configuration.Options;

public sealed class ForgotPasswordMailSendingOption : AppOptions
{
    public string MailTemplateRelativePath { get; init; }

    public string VerifyResetPasswordLink { get; init; }

    public override void Bind(IConfiguration configuration)
    {
        configuration
            .GetRequiredSection("Others")
            .GetRequiredSection("ForgotPasswordMailSending")
            .Bind(this);
    }
}
