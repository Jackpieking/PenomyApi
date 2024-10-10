using Microsoft.Extensions.Configuration;
using PenomyAPI.Infra.Configuration.Common;

namespace PenomyAPI.Infra.Configuration.Options;

public sealed class MailCowSendingOption : AppOptions
{
    public string DisplayName { get; init; }

    public string Host { get; init; }

    public string Password { get; init; }

    public int Port { get; init; }

    public string Mail { get; init; }

    public override void Bind(IConfiguration configuration)
    {
        configuration.GetRequiredSection("").Bind(this);

        Validate();
    }

    private void Validate()
    {
        if (
            string.IsNullOrWhiteSpace(DisplayName)
            || string.IsNullOrWhiteSpace(Host)
            || string.IsNullOrWhiteSpace(Password)
            || Port <= 0
            || string.IsNullOrWhiteSpace(Mail)
        )
        {
            throw new AppOptionBindingException(message: "MailCowSendingOption is not valid.");
        }
    }
}
