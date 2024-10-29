using System;
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

    public async Task<bool> RateArtworkAsync(long userId, long artworkId, byte starRates, CancellationToken token)
    {
        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);
        return await executionStrategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync(token);
            try
            {
                // Find existing rating by the user for this artwork
                var existingRating = await _userRatingArtwork
                    .FirstOrDefaultAsync(a => a.UserId == userId && a.ArtworkId == artworkId, token);

                var artworkMetadata = await _artworkMetaData.FirstOrDefaultAsync(a => a.ArtworkId == artworkId, token);
                if (artworkMetadata == null) return false; // Exit if no metadata exists for the artwork

                if (existingRating == null)
                {
                    _userRatingArtwork.Add(new UserRatingArtwork
                        { UserId = userId, ArtworkId = artworkId, StarRates = starRates });

                    artworkMetadata.TotalUsersRated += 1;
                    artworkMetadata.TotalStarRates += starRates;
                }
                else
                {
                    artworkMetadata.TotalStarRates =
                        artworkMetadata.TotalStarRates - existingRating.StarRates + starRates;

                    existingRating.StarRates = starRates;
                }

                artworkMetadata.AverageStarRate =
                    (double)artworkMetadata.TotalStarRates / artworkMetadata.TotalUsersRated;

                // Save changes
                await _dbContext.SaveChangesAsync(token);
                await transaction.CommitAsync(token);

                return true;
            }
            catch (Exception)
            {
                // Rollback if an error occurs
                await transaction.RollbackAsync(token);
                return false;
            }
        });
    }
}
