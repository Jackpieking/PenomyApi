using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic
{
    public class G62Repository : IG62Repository
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<UserFollowedCreator> _userFollowedCreators;
        private readonly DbSet<UserProfile> _userProfiles;
        private readonly DbSet<CreatorProfile> _creatorProfiles;

        public G62Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _userFollowedCreators = dbContext.Set<UserFollowedCreator>();
            _userProfiles = dbContext.Set<UserProfile>();
            _creatorProfiles = dbContext.Set<CreatorProfile>();
        }

        public async Task<bool> UnfollowCreator(long userId, long creatorId, CancellationToken ct)
        {
            await _dbContext.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
            {
                using var transaction = await _dbContext.Database.BeginTransactionAsync(ct);

                await _userProfiles.Where(o => o.UserId == userId)
                    .ExecuteUpdateAsync(
                        setters => setters.
                            SetProperty(o => o.TotalFollowedCreators, o => o.TotalFollowedCreators - 1));

                await _creatorProfiles.Where(o => o.CreatorId == creatorId)
                    .ExecuteUpdateAsync(
                        setters => setters.
                            SetProperty(o => o.TotalFollowers, o => o.TotalFollowers - 1));

                await _userFollowedCreators.Where(o => o.UserId == userId && o.CreatorId == creatorId)
                    .ExecuteDeleteAsync();

                transaction.Commit();
            });

            return true;
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
    }
}
