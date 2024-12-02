using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

public interface ISM49Repository
{
    Task<bool> IsFriendRequestExistsAsync(long userId, long friendId, CancellationToken token);
    Task<bool> IsUserFriendExistsAsync(long userId, long friendId, CancellationToken token);
    Task<bool> AcceptFriendRequestAsync(IEnumerable<UserFriend> userFriend, CancellationToken token);
}
