using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class SM8Repository : ISM8Repository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<SocialGroup> _socialGroupDbSet;

    public SM8Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _socialGroupDbSet = dbContext.Set<SocialGroup>();
    }

    public async Task<long> CreateSocialGroupAsync(SocialGroup socialGroup)
    {
        try
        {
            _socialGroupDbSet.Add(socialGroup);
            await _dbContext.SaveChangesAsync();
            return socialGroup.Id;
        }
        catch
        {
            return -1;
        }
    }
}
