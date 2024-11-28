using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.DataSeedings.Roles;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia.Common;
using PenomyAPI.Domain.RelationalDb.Entities.UserIdentity;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

public class SM40Repository : ISM40Repository
{
    private readonly DbSet<SocialGroupMember> _socialGroupMemberDbSet;

    public SM40Repository(DbContext dbContext)
    {
        _socialGroupMemberDbSet = dbContext.Set<SocialGroupMember>();
    }

    public async Task<bool> CheckMemberRoleAsync(long groupId, long memberId, CancellationToken ct)
    {
        return await _socialGroupMemberDbSet
            .Where(m =>
                m.GroupId == groupId
                && m.MemberId == memberId
                && m.RoleId == UserRoles.GroupManager.Id
            )
            .AnyAsync(ct);
    }

    public async Task<bool> ChangeGroupMemberRoleAsync(
        long groupId,
        long memberId,
        CancellationToken ct
    )
    {
        try
        {
            await _socialGroupMemberDbSet
                .Where(m => m.GroupId == groupId && m.MemberId == memberId)
                .ExecuteUpdateAsync(
                    setters =>
                        setters.SetProperty(
                            m => m.RoleId,
                            m =>
                                m.RoleId == UserRoles.GroupManager.Id
                                    ? UserRoles.GroupMember.Id
                                    : UserRoles.GroupManager.Id
                        ),
                    ct
                );
            return true;
        }
        catch
        {
            return false;
        }
    }
}
