using Microsoft.AspNetCore.SignalR;
using PenomyAPI.App.Common.Realtime;

namespace PenomyAPI.Realtime.SignalR
{
    public class NotificationHub : Hub<INotificationClient>, INotificationHub
    {
        public const string connectPath = "/signalr/notification";

        public override async Task OnConnectedAsync()
        {
            await Clients.Client(Context.ConnectionId).ReceiveNotification(
                $"{Context.User?.Identity?.Name} is connecting");

            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendToClientAsync(string clientId, string message)
        {
            await Clients.Client(clientId).ReceiveNotification(message);
        }

        public async Task SendToClientsAsync(IReadOnlyList<string> clientIds, string message)
        {

            await Clients.Clients(clientIds).ReceiveNotification(message);
        }

        public async Task SendToGroupAsync(string groupId, string message)
        {
            await Clients.Group(groupId).ReceiveNotification(message);
        }

        public async Task SendToGroupsAsync(IReadOnlyList<string> groupIds, string message)
        {
            await Clients.Groups(groupIds).ReceiveNotification(message);
        }
    }
}
