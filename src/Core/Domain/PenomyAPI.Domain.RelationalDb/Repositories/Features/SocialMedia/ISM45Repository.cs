using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

public interface ISM45Repository
{
    Task<bool> CancelJoinGroupRequestAsync(long groupId,long userId, CancellationToken token);
}
