using PenomyAPI.Infra.Configuration.Common;

namespace PenomyAPI.Infra.Configuration.Options.Base;

public abstract class MailOptions : AppOptions
{
    public string DisplayName { get; init; }

    public string Host { get; init; }

    public string Password { get; init; }

    public int Port { get; init; }

    public string Mail { get; init; }
}
