using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PenomyAPI.App.Common.Realtime;

namespace PenomyAPI.Realtime.SignalR;

[Authorize(AuthenticationSchemes = "Bearer")]
public class NotificationHub : Hub<INotificationClient>, INotificationHub
{
    public const string connectPath = "/signalr/notification";

    public override async Task OnConnectedAsync()
    {
        //await Clients.Client(Context.ConnectionId).ReceiveNotification(
        //    $"{Context.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value} is connecting");

        await base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        return base.OnDisconnectedAsync(exception);
    }
    public async Task SendNotifToClient(string userId)
    {
        await Clients.User(userId).ReceiveNotification();
    }

    public async Task SendMsgToClient(string userId, string message)
    {
        await Clients.User(userId).ReceiveMessage(message);
    }

    public async Task SendNotifToClients(IReadOnlyList<string> userIds)
    {
        await Clients.Users(userIds).ReceiveNotification();
    }

    public async Task SendMsgToClients(IReadOnlyList<string> userIds, string message)
    {

        await Clients.Users(userIds).ReceiveMessage(message);
    }
    public async Task SendNotifToGroup(string groupId)
    {
        await Clients.Group(groupId).ReceiveNotification();
    }

    public async Task SendMsgToGroup(string groupId, string message)
    {
        await Clients.Group(groupId).ReceiveMessage(message);
    }
    public async Task SendNotifToGroups(IReadOnlyList<string> groupIds)
    {
        await Clients.Groups(groupIds).ReceiveNotification();
    }

    public async Task SendMsgToGroups(IReadOnlyList<string> groupIds, string message)
    {
        await Clients.Groups(groupIds).ReceiveMessage(message);
    }
}
