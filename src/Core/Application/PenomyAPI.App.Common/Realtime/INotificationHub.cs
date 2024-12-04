using System.Collections.Generic;
using System.Threading.Tasks;

namespace PenomyAPI.App.Common.Realtime;

public interface INotificationHub
{
    /// <summary>
    ///     Send a notification to the client by <paramref name="userId"/>
    /// </summary>
    /// <param name="userId">
    ///     The client's Id get notification from Hub.
    /// </param>
    /// <returns>
    ///     A <see langword="Task"/> that represents the asynchronous invoke.
    /// </returns>
    Task SendNotifToClient(string userId);

    /// <summary>
    ///     Send a notification to the client with <paramref name="message"/>
    ///     by <paramref name="userId"/>
    /// </summary>
    /// <param name="userId">
    ///     The client's Id get notification from Hub.
    /// </param>
    /// <param name="message">
    ///     The message to send to the client.
    /// </param>
    /// <returns>
    ///     A <see langword="Task"/> that represents the asynchronous invoke.
    /// </returns>
    Task SendMsgToClient(string userId, string message);

    /// <summary>
    ///     Send a notification to the client by <paramref name="userIds"/>
    /// </summary>
    /// <param name="userIds">
    ///     The client's Id get notification from Hub.
    /// </param>
    /// <returns>
    ///     A <see langword="Task"/> that represents the asynchronous invoke.
    /// </returns>
    Task SendNotifToClients(IReadOnlyList<string> userIds);

    /// <summary>
    ///     Send a notification to the client with <paramref name="message"/>
    ///     by <paramref name="userIds"/>
    /// </summary>
    /// <param name="userIds">
    ///     The client's Id get notification from Hub.
    /// </param>
    /// <param name="message">
    ///     The message to send to the client.
    /// </param>
    /// <returns>
    ///     A <see langword="Task"/> that represents the asynchronous invoke.
    /// </returns>
    Task SendMsgToClients(IReadOnlyList<string> userIds, string message);

    /// <summary>
    ///     Send a notification to the group by <paramref name="groupId"/>
    /// </summary>
    /// <param name="groupId">
    ///     The group's Id get notification from Hub.
    /// </param>
    /// <returns>
    ///     A <see langword="Task"/> that represents the asynchronous invoke.
    /// </returns>
    Task SendNotifToGroup(string groupId);

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
    Task SendMsgToGroup(string groupId, string message);

    /// <summary>
    ///     Send a notification to the groups with by <paramref name="groupIds"/>
    /// </summary>
    /// <param name="groupIds">
    ///     The group's Id get notification from Hub.
    /// </param>
    /// <returns>
    ///     A <see langword="Task"/> that represents the asynchronous invoke.
    /// </returns>
    Task SendNotifToGroups(IReadOnlyList<string> groupIds);

    /// <summary>
    ///     Send a notification to the groups with <paramref name="message"/>
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
    Task SendMsgToGroups(IReadOnlyList<string> groupIds, string message);
}
