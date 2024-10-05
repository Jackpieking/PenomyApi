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

        public G44Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _userFollowedArtwork = dbContext.Set<UserFollowedArtwork>();
        }

        public async Task<bool> UnFollowArtwork(long userId, long artworkId, ArtworkType artworkType, CancellationToken ct)
        {
            try
            {
                await _userFollowedArtwork
                    .Where(o => o.UserId == userId && o.ArtworkId == artworkId)
                    .ExecuteDeleteAsync(ct);

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
