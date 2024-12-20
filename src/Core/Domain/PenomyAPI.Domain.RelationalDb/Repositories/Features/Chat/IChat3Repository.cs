using PenomyAPI.Domain.RelationalDb.Entities.Chat;
using PenomyAPI.Domain.RelationalDb.Models.Chat.FeatChat10;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Chat;

public interface IChat3Repository
{
    Task<bool> SaveMessageAsync(ChatMessage chat,
        ChatMessageLikeStatistic statistic,
        CancellationToken token);

    Task<ChatGroup> GetChatGroupAsync(long groupId, CancellationToken token);
    Task<bool> IsMemberOfChatGroupAsync(long groupId, long userId, CancellationToken token);
    Task<Chat10UserProfileReadModel> GetUserChatInfoAsync(long userId, CancellationToken token);
}
