using System.Threading.Tasks;

namespace PenomyAPI.Realtime.SignalR;

public interface INotificationClient
{
    Task ReceiveNotification();

    Task ReceiveMessage(string message);
}
