using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Common;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Common;

internal sealed class ArtworkChapterRepository
    : IArtworkChapterRepository
{
    private readonly DbContext _dbContext;

    #region Compiled Queries
    private static readonly Func<DbContext, long, long, Task<bool>> IsChapterAvailableToDisplayByIdCompileQuery;
    private static readonly Func<DbContext, long, long, long, Task<bool>> IsChapterAvailableToDisplayByIdVer2CompileQuery;
    #endregion

    static ArtworkChapterRepository()
    {
        Expression<Func<DbContext, long, long, bool>> IsChapterAvailableToDisplayByIdExpression = (DbContext dbContext, long chapterId, long userId) =>
            dbContext.Set<ArtworkChapter>()
                .Any(chapter =>
                    // Check if current chapter is public for everyone and not being removed.
                    (
                        chapter.Id == chapterId
                        && chapter.PublicLevel == ArtworkPublicLevel.Everyone
                        && chapter.PublishStatus == PublishStatus.Published
                        && !chapter.IsTemporarilyRemoved
                    )
                    // Or if the current chapter is published by the user with the id the same as the input.
                    || (chapter.Id == chapterId && chapter.CreatedBy == userId));

        IsChapterAvailableToDisplayByIdCompileQuery = EF.CompileAsyncQuery(
            IsChapterAvailableToDisplayByIdExpression);

        Expression<Func<DbContext, long, long, long, bool>> IsChapterAvailableToDisplayByIdVer2Expression = (DbContext dbContext, long artworkId, long chapterId, long userId) =>
            dbContext.Set<ArtworkChapter>()
                .Any(chapter =>
                    // Check if current chapter is public for everyone and not being removed.
                    (
                        chapter.Id == chapterId
                        && chapter.ArtworkId == artworkId
                        && chapter.PublicLevel == ArtworkPublicLevel.Everyone
                        && chapter.PublishStatus == PublishStatus.Published
                        && !chapter.IsTemporarilyRemoved
                    )
                    // Or if the current chapter is published by the user with the id the same as the input.
                    || (chapter.Id == chapterId && chapter.CreatedBy == userId));

        IsChapterAvailableToDisplayByIdVer2CompileQuery = EF.CompileAsyncQuery(
            IsChapterAvailableToDisplayByIdVer2Expression);
    }

    public ArtworkChapterRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<bool> IsChapterAvailableToDisplayByIdAsync(
        long chapterId,
        long userId)
    {
        return IsChapterAvailableToDisplayByIdCompileQuery.Invoke(_dbContext, chapterId, userId);
    }

    public Task<bool> IsChapterAvailableToDisplayByIdAsync(
        long artworkId,
        long chapterId,
        long userId)
    {
        return IsChapterAvailableToDisplayByIdVer2CompileQuery.Invoke(_dbContext, artworkId, chapterId, userId);
    }

    public Task<bool> IsChapterExistedByIdAsync(long chapterId, CancellationToken cancellationToken)
    {
        return _dbContext.Set<ArtworkChapter>()
            .AnyAsync(
                chapter => chapter.Id == chapterId,
                cancellationToken);
    }

    public Task<bool> IsChapterBelongedToArtworkByIdAsync(
        long artworkId,
        long chapterId,
        CancellationToken cancellationToken)
    {
        return _dbContext.Set<ArtworkChapter>()
            .AnyAsync(
                predicate: chapter => chapter.Id == chapterId
                    && chapter.ArtworkId == artworkId,
                cancellationToken: cancellationToken);
    }
}
