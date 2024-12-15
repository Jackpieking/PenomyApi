using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Chat;

public interface IChat5Repository
{
    Task<bool> IsMessageExistsAsync(long userId, long messageId, CancellationToken token = default);
    Task<bool> RemoveMessageAsync(long userId, long messageId, CancellationToken token = default);
}
