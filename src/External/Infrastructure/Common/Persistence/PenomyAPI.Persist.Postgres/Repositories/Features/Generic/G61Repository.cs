using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class G61Repository : IG61Repository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<UserFollowedCreator> _userFollowedCreators;
    private readonly DbSet<UserProfile> _userProfiles;
    private readonly DbSet<CreatorProfile> _creatorProfiles;

    public G61Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _userFollowedCreators = dbContext.Set<UserFollowedCreator>();
        _userProfiles = dbContext.Set<UserProfile>();
        _creatorProfiles = dbContext.Set<CreatorProfile>();

    }

    public Task<bool> IsFollowedCreator(long userId, long creatorId, CancellationToken ct)
    {
        return _userFollowedCreators.AnyAsync(
            o => o.UserId == userId && o.CreatorId == creatorId,
            cancellationToken: ct);
    }

    public async Task<bool> FollowCreator(long userId, long creatorId, CancellationToken ct)
    {
        await _dbContext.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken: ct);

            // Add the user follows a creator
            await _userFollowedCreators
                .AddAsync(
                    new UserFollowedCreator
                    {
                        UserId = userId,
                        CreatorId = creatorId,
                        StartedAt = System.DateTime.UtcNow
                    },
                    cancellationToken: ct
                );

            // Update meta data
            await _userProfiles
                .Where(o => o.UserId == userId)
                .ExecuteUpdateAsync(
                    setters => setters
                        .SetProperty(
                            o => o.TotalFollowedCreators,
                            o => o.TotalFollowedCreators + 1
                        ),
                    cancellationToken: ct
                );

            await _creatorProfiles
                .Where(o => o.CreatorId == creatorId)
                .ExecuteUpdateAsync(
                    setters => setters
                        .SetProperty(
                            o => o.TotalFollowers,
                            o => o.TotalFollowers + 1
                        ),
                    cancellationToken: ct
                );

            await _dbContext.SaveChangesAsync(cancellationToken: ct);

            await transaction.CommitAsync(cancellationToken: ct);
        });

        return true;
    }

    public async Task<bool> IsCreator(long creatorId, CancellationToken ct)
    {
        if (await _creatorProfiles.AnyAsync(o => o.CreatorId == creatorId, cancellationToken: ct))
        {
            return true;
        }

        return false;
    }
}
