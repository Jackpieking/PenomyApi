using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.ArtworkCreation;

internal sealed class Art6Repository : IArt6Repository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<ArtworkChapter> _chapters;

    public Art6Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _chapters = dbContext.Set<ArtworkChapter>();
    }

    public async Task<IEnumerable<ArtworkChapter>> GetChaptersByPublishStatusAsync(
        long comicId,
        PublishStatus publishStatus,
        CancellationToken cancellationToken)
    {
        Expression<Func<ArtworkChapter, bool>> filterExpression = (ArtworkChapter chapter) => chapter.ArtworkId == comicId
            && chapter.PublishStatus == PublishStatus.Drafted;

        // If publish status is not drafted, then get both published and scheduled.
        if (publishStatus != PublishStatus.Drafted)
        {
            filterExpression = (ArtworkChapter chapter) => chapter.ArtworkId == comicId
            && chapter.PublishStatus != PublishStatus.Drafted;
        }

        return await _chapters
            .AsNoTracking()
            .Where(filterExpression)
            .Select(chapter => new ArtworkChapter
            {
                Id = chapter.Id,
                Title = chapter.Title,
                UploadOrder = chapter.UploadOrder,
                PublishStatus = chapter.PublishStatus,
                ThumbnailUrl = chapter.ThumbnailUrl,
                AllowComment = chapter.AllowComment,
                CreatedAt = chapter.CreatedAt,
                PublishedAt = chapter.PublishedAt,
                PublicLevel = chapter.PublicLevel,
            })
            .OrderBy(chapter => chapter.UploadOrder)
            .ToListAsync(cancellationToken);
    }
}
