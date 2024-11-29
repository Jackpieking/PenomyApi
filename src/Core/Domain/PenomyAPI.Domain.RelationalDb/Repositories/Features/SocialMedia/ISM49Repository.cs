using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

public interface ISM49Repository
{
    Task<bool> IsFriendRequestExists(long userId, long friendId, CancellationToken token);
}
