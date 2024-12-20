using PenomyAPI.Domain.RelationalDb.Models.Chat.FeatChat10;
using System.Threading.Tasks;

namespace PenomyAPI.App.Common.Realtime;

public interface IChatHub
{
    /// <summary>
    ///     Send a notification to the client by <paramref name="userId"/>
    /// </summary>
    /// <param name="groupId">
    ///     The group's Id get notification from Hub.
    /// </param>
    /// <returns>
    ///     A <see langword="Task"/> that represents the asynchronous invoke.
    /// </returns>
    Task ReceiveGroupMessange(string groupId, Chat10UserProfileReadModel userChat);
}
