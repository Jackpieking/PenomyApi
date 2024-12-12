using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

public interface ISM40Repository
{
    Task<bool> CheckMemberRoleAsync(long groupId, long memberId, CancellationToken ct);
    Task<bool> ChangeGroupMemberRoleAsync(long groupId, long memberId, CancellationToken ct);
}
