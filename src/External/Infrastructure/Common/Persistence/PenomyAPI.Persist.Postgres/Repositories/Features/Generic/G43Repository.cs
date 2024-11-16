using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic
{
    public class G43Repository : IG43Repository
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<UserFollowedArtwork> _userFollowedArtwork;
        private readonly DbSet<ArtworkMetaData> _artworkMetaData;
        private readonly DbSet<Artwork> _artwork;

        public G43Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _userFollowedArtwork = dbContext.Set<UserFollowedArtwork>();
            _artworkMetaData = dbContext.Set<ArtworkMetaData>();
            _artwork = dbContext.Set<Artwork>();
        }

        public async Task<ArtworkType> GetArtworTypeById(long artworkId, CancellationToken ct)
        {
            var artwork = await _artwork
                .AsNoTracking()
                .Where(artwork => artwork.Id == artworkId)
                .Select(artwork => new Artwork
                {
                    ArtworkType = artwork.ArtworkType
                })
                .FirstOrDefaultAsync(cancellationToken: ct);

            if (Equals(artwork, null))
            {
                return ArtworkType.NotFound;
            }

            return artwork.ArtworkType;
        }

        public async Task<bool> CheckFollowedArtwork(long userId, long artworkId, CancellationToken ct)
        {
            return await _userFollowedArtwork
                .AnyAsync(o =>
                    o.ArtworkId == artworkId &&
                    o.UserId == userId,
                    cancellationToken: ct);
        }

        public async Task<bool> FollowArtwork(
            long userId,
            long artworkId,
            ArtworkType artworkType,
            CancellationToken ct
        )
        {
            await _dbContext.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
            {
                var transaction = await _dbContext.Database.BeginTransactionAsync(ct);

                await _userFollowedArtwork.AddAsync(
                new UserFollowedArtwork
                {
                    UserId = userId,
                    ArtworkId = artworkId,
                    ArtworkType = artworkType,
                    StartedAt = DateTime.UtcNow
                }, cancellationToken: ct);

                await _artworkMetaData
                    .Where(o => o.ArtworkId == artworkId)
                    .ExecuteUpdateAsync(o =>
                        o.SetProperty(o => o.TotalFollowers,
                            e => (e.TotalFollowers + 1)),
                            cancellationToken: ct);

                await _dbContext.SaveChangesAsync(cancellationToken: ct);

                await transaction.CommitAsync(ct);
            });

            return true;
        }

    }
}
