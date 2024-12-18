using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Repositories.Helpers;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.ArtworkCreation;

internal class Art22Repository : IArt22Repository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<ArtworkChapter> _chapterDbSet;

    public Art22Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _chapterDbSet = dbContext.Set<ArtworkChapter>();
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
                BelongedArtwork = new Artwork { Title = chapter.BelongedArtwork.Title, },
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

    public Task<PublishStatus> GetCurrentChapterPublishStatusAsync(
        long chapterId,
        CancellationToken cancellationToken)
    {
        return _chapterDbSet
            .AsNoTracking()
            .Where(chapter => chapter.Id == chapterId)
            .Select(chapter => chapter.PublishStatus)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<bool> HasPermissionToUpdateChapterDetailAsync(
        long creatorId,
        long chapterId,
        CancellationToken cancellationToken)
    {
        return _chapterDbSet.AnyAsync(
            chapter => chapter.Id == chapterId && chapter.CreatedBy == creatorId,
            cancellationToken
        );
    }

    public Task<bool> IsChapterTemporarilyRemovedByIdAsync(
        long chapterId,
        CancellationToken cancellationToken)
    {
        return _chapterDbSet.AnyAsync(
            chapter => chapter.Id == chapterId && chapter.IsTemporarilyRemoved,
            cancellationToken
        );
    }

    public async Task<bool> UpdateAnimeChapterAsync(
        bool isChangedFromDraftedToOtherPublishStatus,
        bool isScheduleDateTimeChanged,
        ArtworkChapter chapterDetail,
        ArtworkChapterMedia chapterVideoMedia,
        CancellationToken cancellationToken)
    {
        var result = new Result<bool>(false);

        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);

        await executionStrategy.ExecuteAsync(
            operation: async () =>
                await InternalUpdateChapterAsync(
                    isChangedFromDraftedToOtherPublishStatus,
                    isScheduleDateTimeChanged,
                    updatedDetail: chapterDetail,
                    chapterVideoMedia,
                    cancellationToken: cancellationToken,
                    result: result
                )
        );

        return result.Value;
    }

    private async Task InternalUpdateChapterAsync(
        bool isChangedFromDraftedToOtherPublishStatus,
        bool isScheduleDateTimeChanged,
        ArtworkChapter updatedDetail,
        ArtworkChapterMedia chapterVideoMedia,
        CancellationToken cancellationToken,
        Result<bool> result
    )
    {
        IDbContextTransaction transaction = null;

        try
        {
            transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            // If changed from drafted mode to other mode,
            // then update the current chapter upload order and
            // the current comic last chapter upload order.
            if (isChangedFromDraftedToOtherPublishStatus)
            {
                await _chapterDbSet
                    .Where(chapter => chapter.Id == updatedDetail.Id)
                    .ExecuteUpdateAsync(
                        chapter =>
                            chapter
                                .SetProperty(
                                    chapter => chapter.Title,
                                    chapter => updatedDetail.Title
                                )
                                .SetProperty(
                                    chapter => chapter.Description,
                                    chapter => updatedDetail.Description
                                )
                                .SetProperty(
                                    chapter => chapter.PublicLevel,
                                    chapter => updatedDetail.PublicLevel
                                )
                                .SetProperty(
                                    chapter => chapter.AllowComment,
                                    chapter => updatedDetail.AllowComment
                                )
                                .SetProperty(
                                    chapter => chapter.UploadOrder,
                                    chapter => updatedDetail.UploadOrder
                                )
                                .SetProperty(
                                    chapter => chapter.UpdatedAt,
                                    chapter => updatedDetail.UpdatedAt
                                )
                                .SetProperty(
                                    chapter => chapter.UpdatedBy,
                                    chapter => updatedDetail.UpdatedBy
                                )
                                .SetProperty(
                                    chapter => chapter.PublishedAt,
                                    chapter => updatedDetail.PublishedAt
                                )
                                .SetProperty(
                                    chapter => chapter.PublishStatus,
                                    chapter => updatedDetail.PublishStatus
                                ),
                        cancellationToken
                    );

                await _dbContext
                    .Set<Artwork>()
                    .Where(comic => comic.Id == updatedDetail.ArtworkId)
                    .ExecuteUpdateAsync(
                        comic =>
                            comic.SetProperty(
                                updateDetail => updateDetail.LastChapterUploadOrder,
                                updateDetail => updateDetail.LastChapterUploadOrder + 1
                            ),
                        cancellationToken
                    );
            }
            // If not changed from draft, then check if
            // the current chapter has any change in publish detail.
            else if (isScheduleDateTimeChanged)
            {
                await _chapterDbSet
                    .Where(chapter => chapter.Id == updatedDetail.Id)
                    .ExecuteUpdateAsync(
                        chapter =>
                            chapter
                                .SetProperty(
                                    chapter => chapter.Title,
                                    chapter => updatedDetail.Title
                                )
                                .SetProperty(
                                    chapter => chapter.Description,
                                    chapter => updatedDetail.Description
                                )
                                .SetProperty(
                                    chapter => chapter.PublicLevel,
                                    chapter => updatedDetail.PublicLevel
                                )
                                .SetProperty(
                                    chapter => chapter.AllowComment,
                                    chapter => updatedDetail.AllowComment
                                )
                                .SetProperty(
                                    chapter => chapter.UpdatedAt,
                                    chapter => updatedDetail.UpdatedAt
                                )
                                .SetProperty(
                                    chapter => chapter.UpdatedBy,
                                    chapter => updatedDetail.UpdatedBy
                                )
                                .SetProperty(
                                    chapter => chapter.PublishedAt,
                                    chapter => updatedDetail.PublishedAt
                                )
                                .SetProperty(
                                    chapter => chapter.PublishStatus,
                                    chapter => updatedDetail.PublishStatus
                                ),
                        cancellationToken
                    );
            }
            else
            {
                await _chapterDbSet
                    .Where(chapter => chapter.Id == updatedDetail.Id)
                    .ExecuteUpdateAsync(
                        chapter =>
                            chapter
                                .SetProperty(
                                    chapter => chapter.Title,
                                    chapter => updatedDetail.Title
                                )
                                .SetProperty(
                                    chapter => chapter.Description,
                                    chapter => updatedDetail.Description
                                )
                                .SetProperty(
                                    chapter => chapter.PublicLevel,
                                    chapter => updatedDetail.PublicLevel
                                )
                                .SetProperty(
                                    chapter => chapter.AllowComment,
                                    chapter => updatedDetail.AllowComment
                                )
                                .SetProperty(
                                    chapter => chapter.UpdatedAt,
                                    chapter => updatedDetail.UpdatedAt
                                )
                                .SetProperty(
                                    chapter => chapter.UpdatedBy,
                                    chapter => updatedDetail.UpdatedBy
                                ),
                        cancellationToken
                    );
            }

            // If the thumbnail URL is not null, then update again.
            if (!string.IsNullOrEmpty(updatedDetail.ThumbnailUrl))
            {
                await _chapterDbSet
                    .Where(chapter => chapter.Id == updatedDetail.Id)
                    .ExecuteUpdateAsync(
                        chapter =>
                            chapter.SetProperty(
                                chapter => chapter.ThumbnailUrl,
                                chapter => updatedDetail.ThumbnailUrl
                            ),
                        cancellationToken
                    );
            }

            // Get the chapter media dbset to process update and delete.
            var hasUpdatedInMedia = chapterVideoMedia != null;

            if (hasUpdatedInMedia)
            {
                await _dbContext.Set<ArtworkChapterMedia>()
                    .Where(
                        media => media.ChapterId == updatedDetail.Id
                        && media.MediaType == ArtworkChapterMediaType.Video)
                    .ExecuteUpdateAsync(
                        media => media
                            .SetProperty(
                                media => media.StorageUrl,
                                media => chapterVideoMedia.StorageUrl
                            ),
                        cancellationToken);
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
