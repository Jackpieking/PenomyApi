using System.Collections.Generic;
using System.Threading.Tasks;

namespace PenomyAPI.App.Common.Realtime;

public interface INotificationHub
{
    /// <summary>
    ///     Send a notification to the client with <paramref name="message"/>
    ///     by <paramref name="clientId"/>
    /// </summary>
    /// <param name="clientId">
    ///     The client's Id get notification from Hub.
    /// </param>
    /// <param name="message">
    ///     The message to send to the client.
    /// </param>
    /// <returns>
    ///     A <see langword="Task"/> that represents the asynchronous invoke.
    /// </returns>
    Task SendToClientAsync(string clientId, string message);

    /// <summary>
    ///     Send a notification to the client with <paramref name="message"/>
    ///     by <paramref name="clientIds"/>
    /// </summary>
    /// <param name="clientIds">
    ///     The client's Id get notification from Hub.
    /// </param>
    /// <param name="message">
    ///     The message to send to the client.
    /// </param>
    /// <returns>
    ///     A <see langword="Task"/> that represents the asynchronous invoke.
    /// </returns>
    Task SendToClientsAsync(IReadOnlyList<string> clientIds, string message);

    /// <summary>
    ///     Send a notification to the group with <paramref name="message"/>
    ///     by <paramref name="groupId"/>
    /// </summary>
    /// <param name="groupId">
    ///     The group's Id get notification from Hub.
    /// </param>
    /// <param name="message">
    ///     The message to send to all clients from the group.
    /// </param>
    /// <returns>
    ///     A <see langword="Task"/> that represents the asynchronous invoke.
    /// </returns>
    Task SendToGroupAsync(string groupId, string message);

    /// <summary>
    ///     Send a notification to the group with <paramref name="message"/>
    ///     by <paramref name="groupIds"/>
    /// </summary>
    /// <param name="groupIds">
    ///     The group's Id get notification from Hub.
    /// </param>
    /// <param name="message">
    ///     The message to send to all clients from the group.
    /// </param>
    /// <returns>
    ///     A <see langword="Task"/> that represents the asynchronous invoke.
    /// </returns>
    Task SendToGroupsAsync(IReadOnlyList<string> groupIds, string message);
}
