using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

public class SM45Repository : ISM45Repository
{
    private readonly DbSet<SocialGroupJoinRequest> _socialGroupJoinRequestDbSet;
    private readonly DbContext _dbContext;

    public SM45Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _socialGroupJoinRequestDbSet = dbContext.Set<SocialGroupJoinRequest>();
    }

    public async Task<bool> CancelJoinGroupRequestAsync(
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
}
