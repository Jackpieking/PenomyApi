using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.Domain.RelationalDb.Entities.Chat;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Chat;

public interface IChat2Repository
{
    Task<List<ChatGroup>> GetChatGroupsAsync(long userId, CancellationToken token);
}
