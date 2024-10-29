using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Helpers;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class G61Repository : IG61Repository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<UserFollowedCreator> _userFollowedCreators;
    private readonly DbSet<CreatorProfile> _creatorProfiles;

    public G61Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _userFollowedCreators = dbContext.Set<UserFollowedCreator>();
        _creatorProfiles = dbContext.Set<CreatorProfile>();
    }

    public async Task<bool> IsFollowedCreator(long userId, long creatorId, CancellationToken ct)
    {
        if (await _userFollowedCreators
            .AsNoTracking()
            .AnyAsync(
                o => o.UserId == userId && o.CreatorId == creatorId,
                cancellationToken: ct
            )
        )
        {
            return true;
        }

        return false;
    }

    public async Task<bool> FollowCreator(long userId, long creatorId, CancellationToken ct)
    {
        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);

        await executionStrategy.ExecuteAsync(operation: StartFollowCreator);

        return true;

        async Task StartFollowCreator()
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
            await _dbContext
                .Set<UserProfile>()
                .Where(o => o.UserId == userId)
                .ExecuteUpdateAsync(
                    setters => setters
                        .SetProperty(
                            o => o.TotalFollowedCreators,
                            o => o.TotalFollowedCreators + 1
                        ),
                    cancellationToken: ct
                );

            await _dbContext
                .Set<CreatorProfile>()
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
        }
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
