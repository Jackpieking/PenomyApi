using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

public interface ISM30Repository
{
    Task<bool> IsUserExistAsync(long friendId, CancellationToken token);
    Task<bool> IsAlreadyFriendAsync(long userId, long friendId, CancellationToken token);
    Task<bool> SendFriendRequest(UserFriendRequest userFriendRequest, CancellationToken token);
}
