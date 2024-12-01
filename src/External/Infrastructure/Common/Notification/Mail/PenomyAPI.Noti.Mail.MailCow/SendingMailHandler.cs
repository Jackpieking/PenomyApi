using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using PenomyAPI.App.Common.Mail;
using PenomyAPI.Infra.Configuration.Options;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Noti.Mail.MailCow;

public sealed class SendingMailHandler : ISendingMailHandler
{
    private readonly PenomyNoReplyMailOptions _mailOptions;

    public SendingMailHandler(PenomyNoReplyMailOptions mailOptions)
    {
        _mailOptions = mailOptions;
    }

    public async Task<bool> SendAsync(
        AppMailContent mailContent,
        CancellationToken cancellationToken
    )
    {
        try
        {
            using var smtpClient = new SmtpClient();

            await smtpClient.ConnectAsync(
                _mailOptions.Host,
                _mailOptions.Port,
                SecureSocketOptions.StartTls,
                cancellationToken
            );

            await smtpClient.AuthenticateAsync(
                _mailOptions.Mail,
                _mailOptions.Password,
                cancellationToken
            );

            await smtpClient.SendAsync(
                GenerateMessage(_mailOptions, mailContent),
                cancellationToken
            );

            await smtpClient.DisconnectAsync(true, cancellationToken);

            return true;
        }
        catch
        {
            return false;
        }
    }

    private static MimeMessage GenerateMessage(
        PenomyNoReplyMailOptions options,
        AppMailContent mailContent
    )
    {
        var message = new MimeMessage();

        if (!string.IsNullOrWhiteSpace(options.Mail))
        {
            message.From.Add(new MailboxAddress(options.DisplayName, options.Mail));
        }

        if (!string.IsNullOrWhiteSpace(mailContent.To))
        {
            message.To.Add(MailboxAddress.Parse(mailContent.To));
        }

        if (!string.IsNullOrWhiteSpace(mailContent.Subject))
        {
            message.Subject = mailContent.Subject;
        }

        message.Body = new BodyBuilder { HtmlBody = mailContent.Body }.ToMessageBody();

        return message;
    }
}
