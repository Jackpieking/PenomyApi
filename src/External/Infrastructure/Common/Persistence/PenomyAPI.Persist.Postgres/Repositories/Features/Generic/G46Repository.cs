using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Data.DbContexts;
using PenomyAPI.Persist.Postgres.Data.UserIdentity;
using PenomyAPI.Persist.Postgres.Repositories.Helpers;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class G46Repository : IG46Repository
{
    private readonly DbSet<Artwork> _artworkContext;
    private readonly AppDbContext _dbContext;
    private readonly DbSet<UserFavoriteArtwork> _favoritesContext;
    private readonly DbSet<ArtworkMetaData> _metaDataContext;
    private readonly Lazy<UserManager<PgUser>> _userManager;

    public G46Repository(AppDbContext dbContext, Lazy<UserManager<PgUser>> userManager)
    {
        _favoritesContext = dbContext.Set<UserFavoriteArtwork>();
        _artworkContext = dbContext.Set<Artwork>();
        _userManager = userManager;
        _metaDataContext = dbContext.Set<ArtworkMetaData>();
        _dbContext = dbContext;
    }

    public async Task<long> AddArtworkFavoriteAsync(
        long userId,
        long artworkId,
        CancellationToken cancellationToken = default)
    {
        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);
        long favoriteCount = 0;
        return await executionStrategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var userFavoriteArtwork = new UserFavoriteArtwork
                {
                    UserId = userId,
                    ArtworkId = artworkId,
                    ArtworkType = await GetArtworkTypeAsync(artworkId, cancellationToken),
                    StartedAt = DateTime.UtcNow
                };
                await _favoritesContext.AddAsync(userFavoriteArtwork, cancellationToken);

                await _metaDataContext
                    .Where(meta => meta.ArtworkId == artworkId)
                    .ExecuteUpdateAsync(meta => meta.SetProperty(
                        m => m.TotalFavorites,
                        m => m.TotalFavorites + 1), cancellationToken);

                var artworkMetadata = _metaDataContext.Where(m => m.ArtworkId == artworkId).Select
                    (x => x.TotalFavorites);
                favoriteCount = await artworkMetadata.FirstOrDefaultAsync(cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                // Rollback and dispose the transaction if an error occurs
                await transaction.RollbackAsync(cancellationToken);
                favoriteCount = 0;
            }

            return favoriteCount;
        });
    }

    public async Task<bool> IsAlreadyFavoriteAsync(long userId, long artworkId, CancellationToken token = default)
    {
        return await _favoritesContext.AsNoTracking()
            .AnyAsync(f => f.UserId == userId && f.ArtworkId == artworkId, token);
    }

    public async Task<bool> IsArtworkExistAsync(long artworkId, CancellationToken token = default)
    {
        return await _artworkContext.AsNoTracking().AnyAsync(a => a.Id == artworkId, token);
    }

    public async Task<bool> IsUserActiveAsync(long userId, CancellationToken token = default)
    {
        var user = await _userManager.Value.FindByIdAsync(userId.ToString());
        if (user is null) return false;
        var isLockedOut = await _userManager.Value.IsLockedOutAsync(user);
        if (isLockedOut) return true;
        return true;
    }

    private async Task<ArtworkType> GetArtworkTypeAsync(long artworkId, CancellationToken token = default)
    {
        return await _artworkContext.Where(a => a.Id == artworkId).Select(a => a.ArtworkType)
            .FirstOrDefaultAsync(token);
    }
}
