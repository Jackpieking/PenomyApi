using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

public sealed class SM1Repository : ISM1Repository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<UserProfile> _userProfile;

    public SM1Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _userProfile = _dbContext.Set<UserProfile>();
    }

    public async Task<UserProfile> GetUserFrofileByUserIdAsync(long userId, CancellationToken cancellationToken)
    {
        return await _userProfile
            .FirstOrDefaultAsync(x => x.UserId == userId);
    }
}
