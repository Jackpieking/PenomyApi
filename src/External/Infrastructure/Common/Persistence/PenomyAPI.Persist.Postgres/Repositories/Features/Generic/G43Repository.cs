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

        public async Task<bool> CheckArtworkExist(long artworkId, ArtworkType artworkType, CancellationToken ct)
        {
            if (await _artwork.AsNoTracking().AnyAsync(o => o.Id == artworkId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> CheckFollowedArtwork(long userId, long artworkId, ArtworkType artworkType, CancellationToken ct)
        {
            if (await _userFollowedArtwork.AnyAsync(o => o.ArtworkId == artworkId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> FollowArtwork(
            long userId,
            long artworkId,
            ArtworkType artworkType,
            CancellationToken ct
        )
        {
            try
            {
                await _userFollowedArtwork.AddAsync(
                    new UserFollowedArtwork
                    {
                        UserId = userId,
                        ArtworkId = artworkId,
                        ArtworkType = artworkType,
                        StartedAt = DateTime.UtcNow
                    });

                await _artworkMetaData
                    .Where(o => o.ArtworkId == artworkId)
                    .ExecuteUpdateAsync(o => o.SetProperty(o => o.TotalFollowers, e => (e.TotalFollowers + 1)));

                await _dbContext.SaveChangesAsync();
            }
            catch
            {
                return false;
            }

            return true;
        }

    }
}
