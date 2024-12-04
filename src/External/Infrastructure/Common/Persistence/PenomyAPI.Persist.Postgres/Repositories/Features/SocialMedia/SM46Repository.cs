using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.DataSeedings.Roles;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

public class SM46Repository : ISM46Repository
{
    private readonly DbSet<SocialGroupJoinRequest> _socialGroupJoinRequestDbSet;
    private readonly DbSet<SocialGroupMember> _socialGroupMemberDbSet;
    private readonly DbContext _dbContext;

    public SM46Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _socialGroupMemberDbSet = dbContext.Set<SocialGroupMember>();
        _socialGroupJoinRequestDbSet = dbContext.Set<SocialGroupJoinRequest>();
    }

    public async Task<bool> RejectJoinGroupRequestAsync(
        long groupId,
        long userId,
        CancellationToken ct
    )
    {
        try
        {
            await _socialGroupJoinRequestDbSet
                .Where(o => o.GroupId == groupId && o.CreatedBy == userId)
                .ExecuteDeleteAsync(ct);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> CheckUserRoleAsync(long groupId, long memberId, CancellationToken ct)
    {
        return await _socialGroupMemberDbSet
            .Where(o =>
                o.GroupId == groupId
                && o.MemberId == memberId
                && o.RoleId == UserRoles.GroupManager.Id
            )
            .AnyAsync(ct);
    }
}
