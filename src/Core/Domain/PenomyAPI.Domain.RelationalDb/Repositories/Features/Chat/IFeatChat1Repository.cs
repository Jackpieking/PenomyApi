using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.Domain.RelationalDb.Entities.Chat;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Chat;

public interface IFeatChat1Repository
{
    Task<bool> CreateGroupAsync(ChatGroup group, ChatGroupMember member, CancellationToken token);
}
