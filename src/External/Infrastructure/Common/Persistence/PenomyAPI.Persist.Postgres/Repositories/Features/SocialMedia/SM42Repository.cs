using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

public class SM42Repository : ISM42Repository
{
    private readonly DbSet<SocialGroupJoinRequest> _socialGroupJoinRequestDbSet;

    public SM42Repository(DbContext dbContext)
    {
        _socialGroupJoinRequestDbSet = dbContext.Set<SocialGroupJoinRequest>();
    }

    public async Task<List<SocialGroupJoinRequest>> GetGroupJoinRequestAsync(long groupId)
    {
        try
        {
            // var member = await _socialGroupMemberDbSet
            //     .Where(o => o.GroupId == groupId && o.MemberId == userId)
            //     .FirstOrDefaultAsync();

            // if (member.RoleId == 1)
            //     return null;

            return await _socialGroupJoinRequestDbSet
                .Where(o => o.GroupId == groupId && o.RequestStatus == RequestStatus.Pending)
                .Select(o => new SocialGroupJoinRequest
                {
                    GroupId = o.GroupId,
                    CreatedAt = o.CreatedAt,
                    Creator = o.Creator,
                })
                .AsNoTracking()
                .AsQueryable()
                .ToListAsync();
        }
        catch
        {
            return null;
        }
    }
}
