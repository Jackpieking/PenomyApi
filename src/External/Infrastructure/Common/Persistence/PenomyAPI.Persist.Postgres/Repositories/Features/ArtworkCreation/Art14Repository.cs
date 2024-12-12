using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Persist.Postgres.Repositories.Helpers;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.ArtworkCreation;

internal sealed class Art14Repository : IArt14Repository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<ArtworkChapter> _chapterDbSet;

    public Art14Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _chapterDbSet = dbContext.Set<ArtworkChapter>();
    }

    public Task<bool> IsChapterExistedAsync(
        long chapterId,
        CancellationToken cancellationToken)
    {
        return _chapterDbSet.AnyAsync(
            chapter => chapter.Id == chapterId
                && !chapter.IsTemporarilyRemoved,
            cancellationToken);
    }

    public Task<bool> IsCurrentCreatorHasPermissionAsync(
        long creatorId,
        long chapterId,
        CancellationToken cancellationToken)
    {
        return _chapterDbSet.AnyAsync(
            chapter => chapter.Id == chapterId
                && chapter.CreatedBy == creatorId,
            cancellationToken);
    }

    public async Task<bool> RemoveArtworkChapterByIdAsync(
        long creatorId,
        long artworkId,
        long chapterId,
        CancellationToken cancellationToken)
    {
        var result = new Result<bool>(false);

        var currentChapterUploadOrder = await _chapterDbSet
            .Where(chapter => chapter.Id == chapterId)
            .Select(chapter => chapter.UploadOrder)
            .FirstOrDefaultAsync(cancellationToken);

        var executionStrategy = RepositoryHelper.CreateExecutionStrategy(_dbContext);

        // Check the chapter upload order to update properly between
        // drafted chapter & already published chapter.
        if (currentChapterUploadOrder != ArtworkChapter.DRAFTED_UPLOAD_ORDER)
        {
            await executionStrategy.ExecuteAsync(
                operation: async () =>
                    await InternalRemoveAlreadyPublishedChapterAsync(
                        creatorId: creatorId,
                        artworkId: artworkId,
                        chapterId: chapterId,
                        currentChapterUploadOrder: currentChapterUploadOrder,
                        cancellationToken: cancellationToken,
                        result: result
                    )
            );
        }
        // Execute this if current chapter is in drafted mode.
        else
        {
            await executionStrategy.ExecuteAsync(
                operation: async () =>
                    await InternalRemoveDraftedChapterAsync(
                        creatorId: creatorId,
                        chapterId: chapterId,
                        cancellationToken: cancellationToken,
                        result: result
                    )
            );
        }

        return result.Value;
    }

    private async Task InternalRemoveAlreadyPublishedChapterAsync(
        long creatorId,
        long artworkId,
        long chapterId,
        int currentChapterUploadOrder,
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

            var removedAt = DateTime.UtcNow;

            await _chapterDbSet
                .Where(chapter => chapter.Id == chapterId)
                .ExecuteUpdateAsync(
                    chapter => chapter
                        .SetProperty(
                            chapter => chapter.IsTemporarilyRemoved,
                            chapter => true)
                        .SetProperty(
                            chapter => chapter.TemporarilyRemovedBy,
                            chapter => creatorId)
                        .SetProperty(
                            chapter => chapter.TemporarilyRemovedAt,
                            chapter => removedAt),
                    cancellationToken);

            // Update all the upload order of all chapters that publish
            // after the current chapter based on their upload order.
            await _chapterDbSet
                .Where(
                    chapter => chapter.ArtworkId == artworkId
                    && !chapter.IsTemporarilyRemoved
                    && chapter.PublishStatus != PublishStatus.Drafted
                    && chapter.UploadOrder > currentChapterUploadOrder)
                .ExecuteUpdateAsync(
                    chapter => chapter
                        .SetProperty(
                            chapter => chapter.UploadOrder,
                            chapter => chapter.UploadOrder - 1),
                    cancellationToken);

            await _dbContext.Set<Artwork>()
                .Where(artwork => artwork.Id == artworkId)
                .ExecuteUpdateAsync(
                    artwork => artwork
                        .SetProperty(
                            artwork => artwork.LastChapterUploadOrder,
                            artwork => artwork.LastChapterUploadOrder - 1),
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

    private async Task InternalRemoveDraftedChapterAsync(
        long creatorId,
        long chapterId,
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

            var removedAt = DateTime.UtcNow;

            await _chapterDbSet
                .Where(chapter => chapter.Id == chapterId)
                .ExecuteUpdateAsync(
                    chapter => chapter
                        .SetProperty(
                            chapter => chapter.IsTemporarilyRemoved,
                            chapter => true)
                        .SetProperty(
                            chapter => chapter.TemporarilyRemovedBy,
                            chapter => creatorId)
                        .SetProperty(
                            chapter => chapter.TemporarilyRemovedAt,
                            chapter => removedAt),
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
}
