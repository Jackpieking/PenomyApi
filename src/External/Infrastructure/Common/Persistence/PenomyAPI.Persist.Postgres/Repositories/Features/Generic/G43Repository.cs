using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic
{
    public class G43Repository : IG43Repository
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<UserFollowedArtwork> _userFollowedArtwork;
        private readonly DbSet<ArtworkMetaData> _artworkMetaData;

        public G43Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _userFollowedArtwork = dbContext.Set<UserFollowedArtwork>();
            _artworkMetaData = dbContext.Set<ArtworkMetaData>();
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

                var artWorkMetaData = await _artworkMetaData
                    .FirstOrDefaultAsync(o => o.ArtworkId == artworkId);

                if (artWorkMetaData == null)
                {
                    await _artworkMetaData.AddAsync(new ArtworkMetaData
                    {
                        ArtworkId = artworkId,
                        TotalFollowers = 1
                    });
                }
                else
                {
                    artWorkMetaData.TotalFollowers++;
                }

                _dbContext.SaveChanges();
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
