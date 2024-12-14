using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.Domain.RelationalDb.Entities.Chat;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Chat;

public interface IChat3Repository
{
    Task<bool> SaveMessageAsync(ChatMessage chat,
        ChatMessageLikeStatistic statistic,
        CancellationToken token);

    Task<ChatGroup> GetChatGroupAsync(long groupId, CancellationToken token);
    Task<bool> IsMemberOfChatGroupAsync(long groupId, long userId, CancellationToken token);
}
