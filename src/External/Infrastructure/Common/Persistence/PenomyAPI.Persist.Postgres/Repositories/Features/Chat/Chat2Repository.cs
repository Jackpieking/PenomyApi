using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.Chat;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Chat;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Chat;

public class Chat2Repository : IChat2Repository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<ChatGroup> _groupContext;

    public Chat2Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _groupContext = _dbContext.Set<ChatGroup>();
    }

    public async Task<List<ChatGroup>> GetChatGroupsAsync(long userId, CancellationToken token)
    {
        return await _groupContext
            .Where(g => g.GroupName.Contains(userId.ToString()))
            .Select(x => new ChatGroup
            {
                Id = x.Id,
                GroupName = x.GroupName,
                IsPublic = x.IsPublic,
                CoverPhotoUrl = x.CoverPhotoUrl,
                ChatGroupType = x.ChatGroupType,
                ChatGroupMembers = x.ChatGroupMembers.Select(y => new ChatGroupMember
                {
                    Member = y.Member,
                    MemberId = y.MemberId,
                    RoleId = y.RoleId,
                    JoinedAt = y.JoinedAt,
                }),
            })
            .ToListAsync(token);
    }

    public async Task<ICollection<long>> GetAllJoinedChatGroupIdAsync(long userId, CancellationToken token)
    {
        return await _groupContext.AsNoTracking()
            .Where(c => c.ChatGroupMembers.Any(cm => cm.MemberId == userId))
            .Select(c => c.Id)
            .ToListAsync(cancellationToken: token);
    }
}
