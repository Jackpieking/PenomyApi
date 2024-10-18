using System.Collections.Generic;

namespace PenomyAPI.App.Common.Mail;

/// <summary>
///     Represent the mail content model.
/// </summary>
public sealed class AppMailContent
{
    public string To { get; set; }

    public string Subject { get; set; } = string.Empty;

    public string Body { get; set; } = string.Empty;
}
