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

internal sealed class Art12Repository : IArt12Repository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<ArtworkChapter> _chapterDbSet;

    public Art12Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _chapterDbSet = dbContext.Set<ArtworkChapter>();
    }

    public Task<bool> IsChapterTemporarilyRemovedByIdAsync(
        long chapterId,
        CancellationToken cancellationToken)
    {
        return _chapterDbSet.AnyAsync(
            chapter
                => chapter.Id == chapterId
                && chapter.IsTemporarilyRemoved,
            cancellationToken);
    }

    public Task<bool> IsComicChapterExistedAsync(
        long comicId,
        long chapterId,
        CancellationToken cancellationToken)
    {
        return _chapterDbSet.AnyAsync(
            chapter
                => chapter.Id == chapterId
                && chapter.ArtworkId == comicId,
            cancellationToken);
    }

    public Task<ArtworkChapter> GetChapterDetailByIdAsync(
        long chapterId,
        CancellationToken cancellationToken)
    {
        return _chapterDbSet
            .AsNoTracking()
            .Where(chapter => chapter.Id == chapterId)
            .Select(chapter => new ArtworkChapter
            {
                Id = chapter.Id,
                ArtworkId = chapter.ArtworkId,
                BelongedArtwork = new Artwork
                {
                    Title = chapter.BelongedArtwork.Title,
                },
                Title = chapter.Title,
                ThumbnailUrl = chapter.ThumbnailUrl,
                UploadOrder = chapter.UploadOrder,
                Description = chapter.Description,
                PublishStatus = chapter.PublishStatus,
                PublicLevel = chapter.PublicLevel,
                AllowComment = chapter.AllowComment,
                PublishedAt = chapter.PublishedAt,
                ChapterMedias = chapter.ChapterMedias.Select(media => new ArtworkChapterMedia
                {
                    Id = media.Id,
                    UploadOrder = media.UploadOrder,
                    FileSize = media.FileSize,
                    StorageUrl = media.StorageUrl,
                })
            })
            .AsSplitQuery()
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<bool> HasPermissionToUpdateChapterDetailAsync(
        long creatorId,
        long chapterId,
        CancellationToken cancellationToken)
    {
        return _chapterDbSet.AnyAsync(
            chapter
                => chapter.Id == chapterId
                && chapter.CreatedBy == creatorId,
            cancellationToken);
    }

    public Task<int> GetCurrentUploadOrderByChapterIdAsync(
        long chapterId,
        CancellationToken cancellationToken)
    {
        return _chapterDbSet
            .AsNoTracking()
            .Where(chapter => chapter.Id == chapterId)
            .Select(chapter => chapter.UploadOrder)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> UpdateComicChapterAsync(
        bool changeFromDrafted,
        bool updateContentOnly,
        ArtworkChapter chapterDetail,
        IEnumerable<ArtworkChapterMedia> updatedChapterMediaItems,
        IEnumerable<long> deletedChapterMediaIds,
        IEnumerable<ArtworkChapterMedia> createdNewChapterMediaItems,
        CancellationToken cancellationToken)
    {
        var result = new Result<bool>(false);

        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);

        await executionStrategy.ExecuteAsync(
            operation: async () =>
                await InternalUpdateComicChapterAsync(
                    changeFromDrafted: changeFromDrafted,
                    updateContentOnly: updateContentOnly,
                    updatedDetail: chapterDetail,
                    updatedChapterMediaItems: updatedChapterMediaItems,
                    deletedChapterMediaIds: deletedChapterMediaIds,
                    createdNewChapterMediaItems: createdNewChapterMediaItems,
                    cancellationToken: cancellationToken,
                    result: result
                )
        );

        return result.Value;
    }

    private async Task InternalUpdateComicChapterAsync(
        bool changeFromDrafted,
        bool updateContentOnly,
        ArtworkChapter updatedDetail,
        IEnumerable<ArtworkChapterMedia> updatedChapterMediaItems,
        IEnumerable<long> deletedChapterMediaIds,
        IEnumerable<ArtworkChapterMedia> createdNewChapterMediaItems,
        CancellationToken cancellationToken,
        Result<bool> result)
    {
        IDbContextTransaction transaction = null;

        try
        {
            transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            // Update the chapter detail information.
            if (updateContentOnly)
            {
                await _chapterDbSet
                .Where(chapter => chapter.Id == updatedDetail.Id)
                .ExecuteUpdateAsync(
                    chapter => chapter
                        .SetProperty(
                            chapter => chapter.Title,
                            chapter => updatedDetail.Title)
                        .SetProperty(
                            chapter => chapter.Description,
                            chapter => updatedDetail.Description)
                        .SetProperty(
                            chapter => chapter.PublicLevel,
                            chapter => updatedDetail.PublicLevel)
                        .SetProperty(
                            chapter => chapter.AllowComment,
                            chapter => updatedDetail.AllowComment)
                        .SetProperty(
                            chapter => chapter.UpdatedAt,
                            chapter => updatedDetail.UpdatedAt)
                        .SetProperty(
                            chapter => chapter.UpdatedBy,
                            chapter => updatedDetail.UpdatedBy),
                    cancellationToken);
            }
            else
            {
                await _chapterDbSet
                    .Where(chapter => chapter.Id == updatedDetail.Id)
                    .ExecuteUpdateAsync(
                        chapter => chapter
                            .SetProperty(
                                chapter => chapter.Title,
                                chapter => updatedDetail.Title)
                            .SetProperty(
                                chapter => chapter.Description,
                                chapter => updatedDetail.Description)
                            .SetProperty(
                                chapter => chapter.UploadOrder,
                                chapter => updatedDetail.UploadOrder)
                            .SetProperty(
                                chapter => chapter.PublicLevel,
                                chapter => updatedDetail.PublicLevel)
                            .SetProperty(
                                chapter => chapter.AllowComment,
                                chapter => updatedDetail.AllowComment)
                            .SetProperty(
                                chapter => chapter.UpdatedAt,
                                chapter => updatedDetail.UpdatedAt)
                            .SetProperty(
                                chapter => chapter.UpdatedBy,
                                chapter => updatedDetail.UpdatedBy)
                            .SetProperty(
                                chapter => chapter.PublishedAt,
                                chapter => updatedDetail.PublishedAt)
                            .SetProperty(
                                chapter => chapter.PublishStatus,
                                chapter => updatedDetail.PublishStatus),
                        cancellationToken);
            }

            // If changed from drafted mode to other mode,
            // then update the last chapter upload order
            if (changeFromDrafted)
            {
                await _dbContext.Set<Artwork>()
                    .Where(comic => comic.Id == updatedDetail.ArtworkId)
                    .ExecuteUpdateAsync(
                        comic => comic
                            .SetProperty(
                                comic => comic.LastChapterUploadOrder,
                                comic => comic.LastChapterUploadOrder + 1),
                        cancellationToken);
            }

            // Get the chapter media dbset to process update and delete.
            var hasUpdatedInMediaItems =
                !Equals(updatedChapterMediaItems, null)
                || !Equals(deletedChapterMediaIds, null)
                || !Equals(createdNewChapterMediaItems, null);

            if (hasUpdatedInMediaItems)
            {
                var chapterMediaDbSet = _dbContext.Set<ArtworkChapterMedia>();

                // If updatedChapterMediaItems is not null then update. 
                if (!Equals(updatedChapterMediaItems, null))
                {
                    foreach (var updatedItem in updatedChapterMediaItems)
                    {
                        await chapterMediaDbSet
                            .Where(chapterMediaItem => chapterMediaItem.Id == updatedItem.Id)
                            .ExecuteUpdateAsync(chapterMediaItem => chapterMediaItem
                                .SetProperty(
                                    chapterMediaItem => chapterMediaItem.UploadOrder,
                                    chapterMediaItem => updatedItem.UploadOrder),
                            cancellationToken);
                    }
                }

                // If deletedChapterMediaIds is not null then update. 
                if (!Equals(deletedChapterMediaIds, null))
                {
                    await chapterMediaDbSet
                        .Where(mediaItem => deletedChapterMediaIds.Contains(mediaItem.Id))
                        .ExecuteDeleteAsync(cancellationToken);
                }

                // If createdNewChapterMediaItems is not null, then persist them to database.
                if (!Equals(createdNewChapterMediaItems, null))
                {
                    await chapterMediaDbSet.AddRangeAsync(
                        createdNewChapterMediaItems,
                        cancellationToken);
                }
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

    public Task<List<ArtworkChapterMedia>> GetChapterMediasByChapterIdAsync(
        long chapterId,
        CancellationToken cancellationToken)
    {
        return _dbContext
            .Set<ArtworkChapterMedia>()
            .AsNoTracking()
            .Where(chapterMedia => chapterMedia.ChapterId == chapterId)
            .Select(chapterMedia => new ArtworkChapterMedia
            {
                Id = chapterMedia.Id,
                UploadOrder = chapterMedia.UploadOrder,
                FileSize = chapterMedia.FileSize,
                StorageUrl = chapterMedia.StorageUrl
            })
            .OrderBy(chapterMedia => chapterMedia.UploadOrder)
            .ToListAsync(cancellationToken);
    }
}
