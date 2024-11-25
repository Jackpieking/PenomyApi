using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

public interface ISM31Repository
{
    Task<bool> IsAlreadyFriendAsync(long userId, long friendId, CancellationToken ct);
    Task<bool> UnfriendAsync(UserFriend userFriend, CancellationToken ct);
    Task<bool> IsUserExistAsync(long friendId, CancellationToken token);
}
