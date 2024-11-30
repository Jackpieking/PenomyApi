using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

public interface ISM41Repository
{
    Task<int> KickMemberAsync(long groupId, long memberId, long userId, CancellationToken ct);
}
