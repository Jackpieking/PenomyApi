using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Repositories.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.ArtworkCreation;

internal sealed class Art17Repository : IArt17Repository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<Artwork> _artworkDbSet;
    private DbSet<ArtworkCategory> _artworkCategoryDbSet;

    public Art17Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _artworkDbSet = dbContext.Set<Artwork>();
    }

    public Task<bool> CheckCreatorPermissionAsync(
        long artworkId,
        long creatoId,
        CancellationToken cancellationToken)
    {
        return _artworkDbSet.AnyAsync(
            anime => anime.Id == artworkId && anime.CreatedBy == creatoId,
            cancellationToken);
    }

    public Task<Artwork> GetAnimeDetailByIdAsync(
        long artworkId,
        CancellationToken cancellationToken)
    {
        return _artworkDbSet
            .AsNoTracking()
            .Where(predicate: artwork => artwork.Id == artworkId)
            .Select(selector: artwork => new Artwork
            {
                Id = artwork.Id,
                Title = artwork.Title,
                ArtworkOriginId = artwork.ArtworkOriginId,
                Introduction = artwork.Introduction,
                ThumbnailUrl = artwork.ThumbnailUrl,
                UpdatedAt = artwork.UpdatedAt,
                PublicLevel = artwork.PublicLevel,
                ArtworkStatus = artwork.ArtworkStatus,
                ArtworkCategories = artwork.ArtworkCategories.Select(
                    artworkCategory => new ArtworkCategory
                    {
                        ArtworkId = artworkCategory.ArtworkId,
                        CategoryId = artworkCategory.CategoryId,
                    }
                ),
                AllowComment = artwork.AllowComment,
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> UpdateAnimeAsync(
        Artwork animeDetail,
        IEnumerable<ArtworkCategory> artworkCategories,
        bool isThumbnailUpdated,
        bool isCategoriesUpdated,
        CancellationToken cancellationToken)
    {
        var result = new Result<bool>(false);

        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);

        await executionStrategy.ExecuteAsync(
            operation: async () =>
                await InternalUpdateAsync(
                    updateDetail: animeDetail,
                    artworkCategories: artworkCategories,
                    isThumbnailUpdated: isThumbnailUpdated,
                    isCategoriesUpdated: isCategoriesUpdated,
                    cancellationToken: cancellationToken,
                    result: result
                )
        );

        return result.Value;
    }

    private async Task InternalUpdateAsync(
        Artwork updateDetail,
        IEnumerable<ArtworkCategory> artworkCategories,
        bool isThumbnailUpdated,
        bool isCategoriesUpdated,
        CancellationToken cancellationToken,
        Result<bool> result
    )
    {
        IDbContextTransaction transaction = null;

        try
        {
            transaction = await RepositoryHelper.CreateTransactionAsync(
                _dbContext,
                cancellationToken
            );

            // Update the detail of the artwork first.
            await _artworkDbSet
                .Where(predicate: updatedAnime => updatedAnime.Id == updateDetail.Id)
                .ExecuteUpdateAsync(
                    setPropertyCalls: updatedAnime =>
                        updatedAnime
                            .SetProperty(anime => anime.Title, updateDetail.Title)
                            .SetProperty(
                                anime => anime.ArtworkOriginId,
                                updateDetail.ArtworkOriginId
                            )
                            .SetProperty(anime => anime.Introduction, updateDetail.Introduction)
                            .SetProperty(anime => anime.ArtworkStatus, updateDetail.ArtworkStatus)
                            .SetProperty(anime => anime.PublicLevel, updateDetail.PublicLevel)
                            .SetProperty(anime => anime.AllowComment, updateDetail.AllowComment)
                            .SetProperty(anime => anime.UpdatedAt, updateDetail.UpdatedAt),
                    cancellationToken: cancellationToken
                );

            // If thumbnail is updated, then update the thumbnail url of the anime.
            if (isThumbnailUpdated)
            {
                await _artworkDbSet
                    .Where(predicate: updatedAnime => updatedAnime.Id == updateDetail.Id)
                    .ExecuteUpdateAsync(
                        setPropertyCalls: updatedAnime =>
                            updatedAnime.SetProperty(
                                anime => anime.ThumbnailUrl,
                                updateDetail.ThumbnailUrl
                            ),
                        cancellationToken: cancellationToken
                    );
            }

            // Update the category list if it has any update.
            _artworkCategoryDbSet = _dbContext.Set<ArtworkCategory>();

            if (isCategoriesUpdated)
            {
                await _artworkCategoryDbSet
                    .Where(predicate: animeCategory => animeCategory.ArtworkId == updateDetail.Id)
                    .ExecuteDeleteAsync(cancellationToken: cancellationToken);

                await _artworkCategoryDbSet.AddRangeAsync(
                    entities: artworkCategories,
                    cancellationToken: cancellationToken
                );
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

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
}
