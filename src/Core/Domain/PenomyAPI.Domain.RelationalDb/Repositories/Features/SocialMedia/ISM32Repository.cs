using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

public interface ISM32Repository
{
    Task<IEnumerable<UserProfile>> GetAllUserFriendsAsync(
        long userId,
        long friendId,
        CancellationToken token
    );
    Task<IEnumerable<long>> GetAllUserFriendRequestAsync(long userId, CancellationToken token);

    Task<IEnumerable<long>> GetUserFriendRequestAsync(long userId, CancellationToken token);

    Task<IEnumerable<UserProfile>> GetAllUserProfilesAsync(long userId, CancellationToken token);
}
