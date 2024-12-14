using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

public class SM39Repository : ISM39Repository
{
    private readonly DbSet<SocialGroupMember> _groupMemberDbSet;

    public SM39Repository(DbContext dbContext)
    {
        _groupMemberDbSet = dbContext.Set<SocialGroupMember>();
    }

    public async Task<IEnumerable<SocialGroupMember>> GetGroupMemberAsync(
        long groupId,
        CancellationToken ct
    )
    {
        return await _groupMemberDbSet
            .Where(m => m.GroupId == groupId)
            .AsNoTracking()
            .AsQueryable()
            .Select(m => new SocialGroupMember
            {
                RoleId = m.RoleId,
                JoinedAt = m.JoinedAt,
                Member = new UserProfile
                {
                    UserId = m.MemberId,
                    NickName = m.Member.NickName,
                    AvatarUrl = m.Member.AvatarUrl,
                    LastActiveAt = m.Member.LastActiveAt,
                },
                Group = new SocialGroup { CreatedBy = m.Group.CreatedBy },
            })
            .ToListAsync(ct);
    }
}
