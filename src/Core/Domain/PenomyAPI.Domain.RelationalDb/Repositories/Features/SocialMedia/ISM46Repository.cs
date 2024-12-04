using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

public interface ISM46Repository
{
    Task<bool> RejectJoinGroupRequestAsync(long groupId,long userId, CancellationToken token);
    Task<bool> CheckUserRoleAsync(long groupId,long userId, CancellationToken token);
}
