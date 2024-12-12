using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PenomyAPI.App.Common.Realtime;

namespace PenomyAPI.Realtime.SignalR;

[Authorize(AuthenticationSchemes = "Bearer")]
public class NotificationHub : Hub<INotificationClient>, INotificationHub
{
    public const string connectPath = "/signalr/notification";
    public readonly IHubContext<NotificationHub, INotificationClient> _hubContext;

    public NotificationHub(IHubContext<NotificationHub, INotificationClient> hubContext)
    {
        _hubContext = hubContext;
    }

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
        await _hubContext.Clients.User(userId).ReceiveNotification();
    }

    public async Task SendMsgToClient(string userId, string message)
    {
        await _hubContext.Clients.User(userId).ReceiveMessage(message);
    }

    public async Task SendNotifToClients(IReadOnlyList<string> userIds)
    {
        await _hubContext.Clients.Users(userIds).ReceiveNotification();
    }

    public async Task SendMsgToClients(IReadOnlyList<string> userIds, string message)
    {

        await _hubContext.Clients.Users(userIds).ReceiveMessage(message);
    }
    public async Task SendNotifToGroup(string groupId)
    {
        await _hubContext.Clients.Group(groupId).ReceiveNotification();
    }

    public async Task SendMsgToGroup(string groupId, string message)
    {
        await _hubContext.Clients.Group(groupId).ReceiveMessage(message);
    }
    public async Task SendNotifToGroups(IReadOnlyList<string> groupIds)
    {
        await _hubContext.Clients.Groups(groupIds).ReceiveNotification();
    }

    public async Task SendMsgToGroups(IReadOnlyList<string> groupIds, string message)
    {
        await _hubContext.Clients.Groups(groupIds).ReceiveMessage(message);
    }
}
