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

public class G47Repository : IG47Repository
{
    private readonly DbSet<Artwork> _artworkContext;
    private readonly AppDbContext _dbContext;
    private readonly DbSet<UserFavoriteArtwork> _favoritesContext;
    private readonly DbSet<ArtworkMetaData> _metaDataContext;
    private readonly Lazy<UserManager<PgUser>> _userManager;

    public G47Repository(AppDbContext dbContext, Lazy<UserManager<PgUser>> userManager)
    {
        _favoritesContext = dbContext.Set<UserFavoriteArtwork>();
        _artworkContext = dbContext.Set<Artwork>();
        _userManager = userManager;
        _metaDataContext = dbContext.Set<ArtworkMetaData>();
        _dbContext = dbContext;
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

    public async Task<bool> RemoveFromFavoriteAsync(long userId, long artworkId, CancellationToken token = default)
    {
        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);

        return await executionStrategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync(token);

            try
            {
                var userFavoriteArtwork = await _favoritesContext
                    .FirstOrDefaultAsync(f => f.UserId == userId && f.ArtworkId == artworkId, token);

                _favoritesContext.Remove(userFavoriteArtwork);

                await _metaDataContext
                    .Where(meta => meta.ArtworkId == artworkId)
                    .ExecuteUpdateAsync(
                        meta => meta.SetProperty(
                            m => m.TotalFavorites,
                            m => m.TotalFavorites - 1),
                        token);

                await _dbContext.SaveChangesAsync(token);

                await transaction.CommitAsync(token);

                return true;
            }
            catch (Exception)
            {
                // Rollback and dispose the transaction if an error occurs
                await transaction.RollbackAsync(token);
                await transaction.DisposeAsync();

                return false;
            }
        });
    }
}
