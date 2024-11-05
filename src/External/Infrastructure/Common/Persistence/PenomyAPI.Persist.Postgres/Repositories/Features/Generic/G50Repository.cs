using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Helpers;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class G50Repository : IG50Repository
{
    private readonly DbSet<Artwork> _artwork;
    private readonly DbSet<ArtworkMetaData> _artworkMetaData;
    private readonly DbContext _dbContext;
    private readonly DbSet<UserRatingArtwork> _userRatingArtwork;

    public G50Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _userRatingArtwork = dbContext.Set<UserRatingArtwork>();
        _artwork = dbContext.Set<Artwork>();
        _artworkMetaData = dbContext.Set<ArtworkMetaData>();
    }

    /// <summary>
    ///     Revokes the start action for the specified user and artwork.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="artworkId">The ID of the artwork.</param>
    /// <param name="token">A cancellation token for the operation.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation, containing a boolean value indicating the success of the
    ///     operation.
    /// </returns>
    public async Task<bool> RevokeStarForArtworkAsync(long userId, long artworkId, CancellationToken token)
    {
        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);
        return await executionStrategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync(token);
            try
            {
                var existingRating = await _userRatingArtwork
                    .Where(a => a.UserId == userId && a.ArtworkId == artworkId)
                    .Select(a => new { a.StarRates })
                    .FirstOrDefaultAsync(token);

                if (existingRating == null)
                    return false;

                await _userRatingArtwork
                    .Where(a => a.UserId == userId && a.ArtworkId == artworkId)
                    .ExecuteDeleteAsync(token);

                await _artworkMetaData
                    .Where(a => a.ArtworkId == artworkId)
                    .ExecuteUpdateAsync(update => update
                            .SetProperty(a => a.TotalUsersRated, a => a.TotalUsersRated > 0 ? a.TotalUsersRated - 1 : 0)
                            .SetProperty(a => a.TotalStarRates, a => a.TotalStarRates - existingRating.StarRates)
                        , token);

                await _artworkMetaData
                    .Where(a => a.ArtworkId == artworkId)
                    .ExecuteUpdateAsync(update => update
                            .SetProperty(a => a.AverageStarRate,
                                a => a.TotalUsersRated > 0
                                    ? (double)a.TotalStarRates / a.TotalUsersRated
                                    : 0)
                        , token);

                await transaction.CommitAsync(token);

                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(token);
                return false;
            }
        });
    }
}
