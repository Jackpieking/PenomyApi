using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

public class SM5Repository : ISM5Repository
{
    private readonly DbSet<SocialGroup> _socialGroupDbSet;

    public SM5Repository(DbContext dbContext)
    {
        _socialGroupDbSet = dbContext.Set<SocialGroup>();
    }

    public async Task<SocialGroup> GetGroupDetailAsync(long userId, long groupId)
    {
        try
        {
            return await _socialGroupDbSet
                .Where(g => g.Id == groupId)
                .Select(g => new SocialGroup
                {
                    Id = g.Id,
                    Name = g.Name,
                    Description = g.Description,
                    CoverPhotoUrl = g.CoverPhotoUrl,
                    IsPublic = g.IsPublic,
                    TotalMembers = g.TotalMembers,
                    CreatedAt = g.CreatedAt,
                    GroupMembers = g.GroupMembers.Where(m => m.MemberId == userId).ToList(),
                    Creator = g.Creator,
                    SocialGroupJoinRequests = g
                        .SocialGroupJoinRequests.Where(m =>
                            m.CreatedBy == userId && m.GroupId == groupId
                        )
                        .ToList(),
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
        catch
        {
            return null;
        }
    }
}
