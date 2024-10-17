namespace PenomyAPI.App.Common.Mail;

/// <summary>
///     Represent the mail content model.
/// </summary>
public sealed class AppMailContent
{
    public string From { get; set; }

    public string To { get; set; }

    public string Cc { get; set; }

    public string Subject { get; set; }

    public string Body { get; set; }
}
