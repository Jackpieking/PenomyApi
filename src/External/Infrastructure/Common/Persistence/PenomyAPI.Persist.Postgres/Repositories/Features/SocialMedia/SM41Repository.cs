using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

public class SM41Repository : ISM41Repository
{
    private readonly DbSet<SocialGroupMember> _socialGroupMemberDbSet;

    public SM41Repository(DbContext dbContext)
    {
        _socialGroupMemberDbSet = dbContext.Set<SocialGroupMember>();
    }

    public async Task<int> KickMemberAsync(long groupId, long userId)
    {
        return await _socialGroupMemberDbSet
            .Where(o => o.GroupId == groupId && o.MemberId == userId)
            .ExecuteDeleteAsync();
    }
}
