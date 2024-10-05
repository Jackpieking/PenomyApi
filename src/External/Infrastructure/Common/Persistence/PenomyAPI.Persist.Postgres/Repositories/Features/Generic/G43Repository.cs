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

        public G43Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _userFollowedArtwork = dbContext.Set<UserFollowedArtwork>();
        }

        public async Task<bool> FollowArtwork(long userId, long artworkId, ArtworkType artworkType, CancellationToken ct)
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
