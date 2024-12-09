using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Models.ArtworkCreation.FeatArt3;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Repositories.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.ArtworkCreation;

internal sealed class Art3Repository : IArt3Repository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<Artwork> _artworkDbSet;

    public Art3Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _artworkDbSet = dbContext.Set<Artwork>();
    }

    public Task<bool> AreArtworksTemporarilyRemovedAsync(
        IEnumerable<long> artworkIds,
        long creatorId,
        CancellationToken cancellationToken)
    {
        return _artworkDbSet
            .Where(artwork => artworkIds.Contains(artwork.Id))
            .AllAsync(
                artwork => artwork.IsTemporarilyRemoved
                && artwork.ArtworkStatus != ArtworkStatus.PermanentlyRemoved
                && artwork.CreatedBy == creatorId,
            cancellationToken);
    }

    public async Task<Art3CheckDeletedItemReadModel> CheckCurrentCreatorDeletedItemsAsync(
        long creatorId,
        CancellationToken cancellationToken
    )
    {
        int totalDeletedArtworks = await _artworkDbSet
            .Where(
                artwork => artwork.CreatedBy == creatorId
                && artwork.IsTemporarilyRemoved
                && artwork.ArtworkStatus != ArtworkStatus.PermanentlyRemoved
            )
            .CountAsync(cancellationToken);

        if (totalDeletedArtworks == 0)
        {
            return Art3CheckDeletedItemReadModel.Empty;
        }

        var totalDeletedComics = await _dbContext
            .Set<Artwork>()
            .Where(artwork =>
                artwork.CreatedBy == creatorId
                && artwork.ArtworkType == ArtworkType.Comic
                && artwork.IsTemporarilyRemoved
            )
            .CountAsync(cancellationToken);

        var totalDeletedAnimes = totalDeletedArtworks - totalDeletedComics;

        return new()
        {
            TotalDeletedComics = totalDeletedComics,
            TotalDeletedAnimes = totalDeletedAnimes,
        };
    }

    public Task<List<Art3DeletedArtworkDetailReadModel>> GetAllDeletedArtworksByTypeAsync(
        long creatorId,
        ArtworkType artworkType,
        CancellationToken cancellationToken
    )
    {
        return _artworkDbSet
            .AsNoTracking()
            .Where(
                artwork => artwork.CreatedBy == creatorId
                && artwork.IsTemporarilyRemoved
                && artwork.ArtworkType == artworkType
                && artwork.ArtworkStatus != ArtworkStatus.PermanentlyRemoved)
            .Select(artwork => new Art3DeletedArtworkDetailReadModel
            {
                Id = artwork.Id,
                Title = artwork.Title,
                PublicLevel = artwork.PublicLevel,
                ArtworkStatus = artwork.ArtworkStatus,
                ThumbnailUrl = artwork.ThumbnailUrl,
                TotalChapters = artwork.LastChapterUploadOrder,
                RemovedAt = artwork.TemporarilyRemovedAt,
                TotalViews = artwork.ArtworkMetaData.TotalViews,
                TotalFollowers = artwork.ArtworkMetaData.TotalFollowers,
                TotalStarRates = artwork.ArtworkMetaData.TotalStarRates,
                TotalUsersRated = artwork.ArtworkMetaData.TotalUsersRated,
            })
            .OrderByDescending(artwork => artwork.RemovedAt)
            .ToListAsync(cancellationToken);
    }

    public Task<bool> HasAnyDeletedItemsWithArtworTypeAsync(
        long creatorId,
        ArtworkType artworkType,
        CancellationToken cancellationToken)
    {
        return _artworkDbSet.AnyAsync(
            artwork => artwork.CreatedBy == creatorId
                && artwork.IsTemporarilyRemoved
                && artwork.ArtworkStatus != ArtworkStatus.PermanentlyRemoved
                && artwork.ArtworkType == artworkType,
            cancellationToken);
    }

    #region PermanentlyRemoveArtworksByIdsAsync section.
    public async Task<bool> PermanentlyRemoveArtworksByIdsAsync(
        IEnumerable<long> artworkIds,
        CancellationToken cancellationToken)
    {
        var result = new Result<bool>(false);

        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);

        await executionStrategy.ExecuteAsync(
            operation: async () =>
                await InternalPermanentlyRemoveArtworksByIdsAsync(
                    artworkIds,
                    cancellationToken: cancellationToken,
                    result: result
                )
        );

        return result.Value;
    }

    private async Task InternalPermanentlyRemoveArtworksByIdsAsync(
        IEnumerable<long> artworkIds,
        CancellationToken cancellationToken,
        Result<bool> result)
    {
        IDbContextTransaction transaction = null;

        try
        {
            transaction = await RepositoryHelper.CreateTransactionAsync(
                _dbContext,
                cancellationToken
            );

            await _artworkDbSet
                .Where(artwork => artworkIds.Contains(artwork.Id))
                .ExecuteUpdateAsync(
                    artwork => artwork
                        .SetProperty(
                            artwork => artwork.ArtworkStatus,
                            artwork => ArtworkStatus.PermanentlyRemoved),
                    cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            result.Value = true;
        }
        catch (System.Exception)
        {
            if (transaction != null)
            {
                await transaction.RollbackAsync(cancellationToken);
                await transaction.DisposeAsync();
            }
        }
    }
    #endregion

    #region RestoreDeletedArtworksByIdsAsync section.
    public async Task<bool> RestoreDeletedArtworksByIdsAsync(
        IEnumerable<long> artworkIds,
        CancellationToken cancellationToken)
    {
        var result = new Result<bool>(false);

        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);

        await executionStrategy.ExecuteAsync(
            operation: async () =>
                await InternalRestoreDeletedArtworksByIdsAsync(
                    artworkIds,
                    cancellationToken: cancellationToken,
                    result: result
                )
        );

        return result.Value;
    }

    private async Task InternalRestoreDeletedArtworksByIdsAsync(
        IEnumerable<long> artworkIds,
        CancellationToken cancellationToken,
        Result<bool> result)
    {
        IDbContextTransaction transaction = null;

        try
        {
            transaction = await RepositoryHelper.CreateTransactionAsync(
                _dbContext,
                cancellationToken
            );

            await _artworkDbSet
                .Where(artwork => artworkIds.Contains(artwork.Id))
                .ExecuteUpdateAsync(
                    artwork => artwork
                        .SetProperty(
                            artwork => artwork.ArtworkStatus,
                            artwork => ArtworkStatus.OnGoing)
                        .SetProperty(
                            artwork => artwork.IsTemporarilyRemoved,
                            artwork => false),
                    cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            result.Value = true;
        }
        catch (System.Exception)
        {
            if (transaction != null)
            {
                await transaction.RollbackAsync(cancellationToken);
                await transaction.DisposeAsync();
            }
        }
    }
    #endregion

    #region RestoreAllDeletedArtworksAsync section
    public async Task<bool> RestoreAllDeletedItemsByArtworkTypeAsync(
        long creatorId,
        ArtworkType artworkType,
        CancellationToken cancellationToken)
    {
        var result = new Result<bool>(false);

        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);

        await executionStrategy.ExecuteAsync(
            operation: async () =>
                await InternalRestoreAllDeletedArtworksAsync(
                    creatorId,
                    artworkType,
                    cancellationToken: cancellationToken,
                    result: result
                )
        );

        return result.Value;
    }

    private async Task InternalRestoreAllDeletedArtworksAsync(
        long creatorId,
        ArtworkType artworkType,
        CancellationToken cancellationToken,
        Result<bool> result)
    {
        IDbContextTransaction transaction = null;

        try
        {
            transaction = await RepositoryHelper.CreateTransactionAsync(
                _dbContext,
                cancellationToken
            );

            await _artworkDbSet
                .Where(
                    artwork => artwork.IsTemporarilyRemoved
                    && artwork.CreatedBy == creatorId
                    && artwork.ArtworkType == artworkType
                )
                .ExecuteUpdateAsync(
                    artwork => artwork
                        .SetProperty(
                            artwork => artwork.ArtworkStatus,
                            artwork => ArtworkStatus.OnGoing)
                        .SetProperty(
                            artwork => artwork.IsTemporarilyRemoved,
                            artwork => false),
                    cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            result.Value = true;
        }
        catch (System.Exception)
        {
            if (transaction != null)
            {
                await transaction.RollbackAsync(cancellationToken);
                await transaction.DisposeAsync();
            }
        }
    }
    #endregion

    #region PermanentlyRemoveAllDeletedArtworksAsync section
    public async Task<bool> PermanentlyRemoveAllDeletedItemsByArtworkTypeAsync(
        long creatorId,
        ArtworkType artworkType,
        CancellationToken cancellationToken)
    {
        var result = new Result<bool>(false);

        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);

        await executionStrategy.ExecuteAsync(
            operation: async () =>
                await InternalPermanentlyRemoveAllDeletedArtworksAsync(
                    creatorId,
                    artworkType,
                    cancellationToken: cancellationToken,
                    result: result
                )
        );

        return result.Value;
    }

    private async Task InternalPermanentlyRemoveAllDeletedArtworksAsync(
        long creatorId,
        ArtworkType artworkType,
        CancellationToken cancellationToken,
        Result<bool> result)
    {
        IDbContextTransaction transaction = null;

        try
        {
            transaction = await RepositoryHelper.CreateTransactionAsync(
                _dbContext,
                cancellationToken
            );

            await _artworkDbSet
                .Where(
                    artwork => artwork.CreatedBy == creatorId
                    && artwork.IsTemporarilyRemoved
                    && artwork.ArtworkType == artworkType
                    && artwork.ArtworkStatus != ArtworkStatus.PermanentlyRemoved
                )
                .ExecuteUpdateAsync(
                    artwork => artwork
                        .SetProperty(
                            artwork => artwork.ArtworkStatus,
                            artwork => ArtworkStatus.PermanentlyRemoved),
                    cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            result.Value = true;
        }
        catch (System.Exception)
        {
            if (transaction != null)
            {
                await transaction.RollbackAsync(cancellationToken);
                await transaction.DisposeAsync();
            }
        }
    }
    #endregion
}
