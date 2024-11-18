using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class SM9Repository : ISM9Repository
{
    private readonly DbSet<SocialGroup> _socialGroupDbSet;

    public SM9Repository(DbContext dbContext)
    {
        _socialGroupDbSet = dbContext.Set<SocialGroup>();
    }

    public async Task<List<SocialGroup>> GetSocialGroupsAsync(long userId)
    {
        try
        {
            var result = await _socialGroupDbSet
            .Where(g => g.CreatedBy == userId)
            .AsQueryable()
            .AsNoTracking()
            .ToListAsync();
        return result;
        } catch{
            return null;
        }
    }
}
