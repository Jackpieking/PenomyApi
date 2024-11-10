using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Data.DbContexts;
using PenomyAPI.Persist.Postgres.Repositories.Helpers;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class G49Repository : IG49Repository
{
    private readonly DbSet<Artwork> _artwork;
    private readonly DbSet<ArtworkMetaData> _artworkMetaData;
    private readonly AppDbContext _dbContext;
    private readonly DbSet<UserRatingArtwork> _userRatingArtwork;

    public G49Repository(AppDbContext context)
    {
        _dbContext = context;
        _artworkMetaData = _dbContext.Set<ArtworkMetaData>();
        _userRatingArtwork = context.Set<UserRatingArtwork>();
        _artwork = context.Set<Artwork>();
    }

    public Task<bool> IsArtworkExistsAsync(long id, CancellationToken cancellationToken)
    {
        return _artwork.AnyAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<ArtworkMetaData> RateArtworkAsync(long userId, long artworkId, byte starRates,
        CancellationToken token)
    {
        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);
        return await executionStrategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync(token);
            try
            {
                // Check if an existing rating by this user for the artwork exists
                var existingRating = await _userRatingArtwork
                    .Select(x => new UserRatingArtwork
                    {
                        ArtworkId = x.ArtworkId,
                        UserId = x.UserId,
                        StarRates = x.StarRates
                    })
                    .FirstOrDefaultAsync(a => a.UserId == userId && a.ArtworkId == artworkId, token);

                if (existingRating == null)
                {
                    // Add a new rating
                    await _userRatingArtwork.AddAsync(new UserRatingArtwork
                        {
                            UserId = userId, ArtworkId = artworkId, StarRates = starRates, RatedAt = DateTime.UtcNow
                        },
                        token);

                    // Increment the TotalUsersRated and add to TotalStarRates
                    await _artworkMetaData
                        .Where(a => a.ArtworkId == artworkId)
                        .ExecuteUpdateAsync(update => update
                                .SetProperty(a => a.TotalUsersRated, a => a.TotalUsersRated + 1)
                                .SetProperty(a => a.TotalStarRates, a => a.TotalStarRates + starRates)
                            , token);
                }
                else
                {
                    // Update TotalStarRates based on the difference between the new and previous star rating
                    await _artworkMetaData
                        .Where(a => a.ArtworkId == artworkId)
                        .ExecuteUpdateAsync(update => update
                                .SetProperty(a => a.TotalStarRates,
                                    a => a.TotalStarRates - existingRating.StarRates + starRates)
                            , token);

                    // Update the user's existing rating with the new star rate
                    await _userRatingArtwork
                        .Where(a => a.UserId == userId && a.ArtworkId == artworkId)
                        .ExecuteUpdateAsync(update => update
                                .SetProperty(a => a.StarRates, starRates)
                                .SetProperty(a => a.RatedAt, DateTime.UtcNow)
                            , token);
                }

                // Calculate and update the new average star rate
                var artworkMetaData = await _artworkMetaData
                    .Where(a => a.ArtworkId == artworkId)
                    .Select(a => new { a.TotalStarRates, a.TotalUsersRated })
                    .FirstOrDefaultAsync(token);

                var updatedAverageStarRate = (double)artworkMetaData.TotalStarRates / artworkMetaData.TotalUsersRated;

                await _artworkMetaData
                    .Where(a => a.ArtworkId == artworkId)
                    .ExecuteUpdateAsync(update => update
                            .SetProperty(a => a.AverageStarRate, updatedAverageStarRate)
                        , token);

                await _dbContext.SaveChangesAsync(token);
                // Commit transaction if all updates are successful
                await transaction.CommitAsync(token);

                // Return the updated average star rate
                return await _artworkMetaData.Where(x => x.ArtworkId == artworkId).Select(x => new ArtworkMetaData
                {
                    TotalStarRates = x.TotalStarRates,
                    TotalUsersRated = x.TotalUsersRated,
                    AverageStarRate = x.AverageStarRate
                }).FirstOrDefaultAsync(token);
            }
            catch (Exception)
            {
                // Rollback if any error occurs
                await transaction.RollbackAsync(token);
                throw;
            }
        });
    }

    public async Task<long> GetCurrentUserRatingAsync(long userId, long artworkId, CancellationToken cancellationToken)
    {
        return await _userRatingArtwork.Where(x => x.UserId == userId && x.ArtworkId == artworkId)
            .Select(x => x.StarRates)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
