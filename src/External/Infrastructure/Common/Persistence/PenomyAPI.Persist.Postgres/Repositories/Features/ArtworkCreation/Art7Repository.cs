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

public sealed class Art7Repository : IArt7Repository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<Category> _categoryDbSet;
    private readonly DbSet<Artwork> _artworkDbSet;
    private readonly DbSet<ArtworkCategory> _artworkCategoryDbSet;
    private readonly DbSet<ArtworkOrigin> _originDbSet;

    public Art7Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _categoryDbSet = dbContext.Set<Category>();
        _artworkDbSet = dbContext.Set<Artwork>();
        _artworkCategoryDbSet = dbContext.Set<ArtworkCategory>();
        _originDbSet = dbContext.Set<ArtworkOrigin>();
    }

    public Task<bool> CheckCreatorPermissionAsync(
        long comicId,
        long creatoId,
        CancellationToken cancellationToken)
    {
        return _artworkDbSet.AnyAsync(
            comic => comic.Id == comicId && comic.CreatedBy == creatoId,
            cancellationToken);
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync(
        CancellationToken cancellationToken
    )
    {
        return await _categoryDbSet
            .AsNoTracking()
            .Select(category => new Category { Id = category.Id, Name = category.Name, })
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ArtworkOrigin>> GetAllOriginsAsync(
        CancellationToken cancellationToken
    )
    {
        return await _originDbSet
            .AsNoTracking()
            .Select(origin => new ArtworkOrigin
            {
                Id = origin.Id,
                CountryName = origin.CountryName,
                ImageUrl = origin.ImageUrl,
            })
            .ToListAsync(cancellationToken);
    }

    public Task<Artwork> GetComicDetailByIdAsync(long comicId, CancellationToken cancellationToken)
    {
        return _artworkDbSet
            .AsNoTracking()
            .Where(predicate: comic => comic.Id == comicId)
            .Select(selector: comic => new Artwork
            {
                Id = comic.Id,
                Title = comic.Title,
                ArtworkOriginId = comic.ArtworkOriginId,
                Introduction = comic.Introduction,
                ThumbnailUrl = comic.ThumbnailUrl,
                UpdatedAt = comic.UpdatedAt,
                PublicLevel = comic.PublicLevel,
                ArtworkStatus = comic.ArtworkStatus,
                ArtworkCategories = comic.ArtworkCategories.Select(
                    artworkCategory => new ArtworkCategory
                    {
                        ArtworkId = artworkCategory.ArtworkId,
                        CategoryId = artworkCategory.CategoryId,
                    }
                ),
                AllowComment = comic.AllowComment,
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<bool> IsComicExistedByIdAsync(long comicId, CancellationToken cancellationToken)
    {
        return _artworkDbSet.AnyAsync(
            predicate: comic => comic.Id == comicId,
            cancellationToken: cancellationToken
        );
    }

    public async Task<bool> UpdateComicAsync(
        Artwork comic,
        IEnumerable<ArtworkCategory> artworkCategories,
        bool isThumbnailUpdated,
        bool isCategoriesUpdated,
        CancellationToken cancellationToken
    )
    {
        var result = new Result<bool>(false);

        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);

        await executionStrategy.ExecuteAsync(
            operation: async () =>
                await InternalUpdateComicAsync(
                    updateDetail: comic,
                    artworkCategories: artworkCategories,
                    isThumbnailUpdated: isThumbnailUpdated,
                    isCategoriesUpdated: isCategoriesUpdated,
                    cancellationToken: cancellationToken,
                    result: result
                )
        );

        return result.Value;
    }

    private async Task InternalUpdateComicAsync(
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
                .Where(predicate: updatedComic => updatedComic.Id == updateDetail.Id)
                .ExecuteUpdateAsync(
                    setPropertyCalls: updatedComic =>
                        updatedComic
                            .SetProperty(comic => comic.Title, updateDetail.Title)
                            .SetProperty(
                                comic => comic.ArtworkOriginId,
                                updateDetail.ArtworkOriginId
                            )
                            .SetProperty(comic => comic.Introduction, updateDetail.Introduction)
                            .SetProperty(comic => comic.ArtworkStatus, updateDetail.ArtworkStatus)
                            .SetProperty(comic => comic.PublicLevel, updateDetail.PublicLevel)
                            .SetProperty(comic => comic.AllowComment, updateDetail.AllowComment)
                            .SetProperty(comic => comic.UpdatedAt, updateDetail.UpdatedAt),
                    cancellationToken: cancellationToken
                );

            // If thumbnail is updated, then update the thumbnail url of the comic.
            if (isThumbnailUpdated)
            {
                await _artworkDbSet
                    .Where(predicate: updatedComic => updatedComic.Id == updateDetail.Id)
                    .ExecuteUpdateAsync(
                        setPropertyCalls: updatedComic =>
                            updatedComic.SetProperty(
                                comic => comic.ThumbnailUrl,
                                updateDetail.ThumbnailUrl
                            ),
                        cancellationToken: cancellationToken
                    );
            }

            // Update the category list if it has any update.
            if (isCategoriesUpdated)
            {
                await _artworkCategoryDbSet
                    .Where(predicate: comicCategory => comicCategory.ArtworkId == updateDetail.Id)
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
