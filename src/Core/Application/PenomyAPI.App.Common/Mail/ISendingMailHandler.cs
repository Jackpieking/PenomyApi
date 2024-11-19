using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.Common.Mail;

/// <summary>
///     Represent interface of sending mail handler.
/// </summary>
public interface ISendingMailHandler
{
    /// <summary>
    ///     Sending an email to the specified user.
    /// </summary>
    /// <param name="mailContent">
    ///     A model contains all receiver information.
    /// </param>
    /// <param name="cancellationToken">
    ///     A token that is used for notifying system
    ///     to cancel the current operation when user stop
    ///     the request.
    /// </param>
    /// <param name="profile">
    ///     Profile for sending mail.
    /// </param>
    /// <returns>
    ///     Task containing boolean result.
    /// </returns>
    Task<bool> SendAsync(AppMailContent mailContent, CancellationToken cancellationToken);
}
