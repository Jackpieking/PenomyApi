using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.DataSeedings.Roles;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

public class SM6Repository : ISM6Repository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<SocialGroup> _socialGroups;
    private readonly DbSet<SocialGroupJoinRequest> _socialGroupJoinRequests;
    private readonly DbSet<SocialGroupMember> _socialGroupsMember;

    public SM6Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _socialGroups = dbContext.Set<SocialGroup>();
        _socialGroupJoinRequests = dbContext.Set<SocialGroupJoinRequest>();
        _socialGroupsMember = dbContext.Set<SocialGroupMember>();
    }

    public async Task<bool> CheckGroupExists(long groupId, CancellationToken ct)
    {
        return await _socialGroups.AsNoTracking()
            .AnyAsync(o => o.Id == groupId, ct);
    }

    public async Task<bool> CheckUserJoinedGroupAsync(long userId, long groupId, CancellationToken ct)
    {
        return await _socialGroupJoinRequests.AsNoTracking()
            .AnyAsync(o =>
                o.CreatedBy == userId &&
                o.GroupId == groupId,
                cancellationToken: ct);
    }

    public async Task<bool> AddUserJoinRequestByUserIdAndGroupIdAsync(long userId, long groupId, CancellationToken ct)
    {
        await _socialGroupJoinRequests.AddAsync(
            new SocialGroupJoinRequest
            {
                CreatedBy = userId,
                GroupId = groupId,
                RequestStatus = RequestStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });

        _dbContext.SaveChanges();

        return true;
    }

    public async Task<bool> AddUserToGroupByUserIdAndGroupIdAsync(long userId, long groupId, CancellationToken ct)
    {
        await _dbContext.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken: ct);

            await _socialGroupsMember.AddAsync(new SocialGroupMember
            {
                GroupId = groupId,
                MemberId = userId,
                RoleId = 0,
                JoinedAt = DateTime.UtcNow
            }, ct);

            await _socialGroups.ExecuteUpdateAsync(o =>
                o.SetProperty(set => set.TotalMembers, o => o.TotalMembers + 1),
                ct);

            await _socialGroupJoinRequests
                .Where(o =>
                    o.CreatedBy == userId &&
                    o.GroupId == groupId)
                .ExecuteDeleteAsync(ct);

            await _dbContext.SaveChangesAsync(ct);

            await transaction.CommitAsync(ct);
        });

        return true;
    }

    public async Task<ICollection<long>> GetGroupManagerByGroupIdAsync(long groupId, CancellationToken ct)
    {
        return await _socialGroupsMember.AsNoTracking()
            .Where(o => o.GroupId == groupId && o.RoleId == UserRoles.GroupManager.Id)
            .Select(o => o.MemberId)
            .ToListAsync(ct);
    }
}
