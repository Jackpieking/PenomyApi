using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

public interface ISM41Repository
{
    Task<bool> CheckRemovableAsync(long groupId, long memberId, CancellationToken ct);
    Task<int> KickMemberAsync(long groupId, long memberId, long userId, CancellationToken ct);
}
