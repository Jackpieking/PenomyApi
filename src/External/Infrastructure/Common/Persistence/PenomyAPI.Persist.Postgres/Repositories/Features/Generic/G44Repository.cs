using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic
{
    public class G44Repository : IG44Repository
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<UserFollowedArtwork> _userFollowedArtwork;
        private readonly DbSet<ArtworkMetaData> _artworkMetaData;

        public G44Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _userFollowedArtwork = dbContext.Set<UserFollowedArtwork>();
            _artworkMetaData = dbContext.Set<ArtworkMetaData>();

        }

        public async Task<bool> UnFollowArtwork(long userId, long artworkId, CancellationToken ct)
        {
            await _dbContext.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
            {
                using var transaction = await _dbContext.Database.BeginTransactionAsync();

                await _userFollowedArtwork
                        .Where(o => o.UserId == userId && o.ArtworkId == artworkId)
                        .ExecuteDeleteAsync(ct);

                await _artworkMetaData
                    .Where(o => o.ArtworkId == artworkId)
                    .ExecuteUpdateAsync(o => o.SetProperty(o => o.TotalFollowers, e => e.TotalFollowers - 1), ct);

                await _dbContext.SaveChangesAsync(ct);

                await transaction.CommitAsync();
            });

            return true;
        }
    }
}
