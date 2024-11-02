using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using System.Collections.Generic;
using System.Linq;
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
        return await _chapters
            .AsNoTracking()
            .Where(chapter =>
                chapter.ArtworkId == comicId
                && chapter.PublishStatus == publishStatus)
            .Select(chapter => new ArtworkChapter
            {
                Id = chapter.Id,
                Title = chapter.Title,
                UploadOrder = chapter.UploadOrder,
                PublishStatus = publishStatus,
                ThumbnailUrl = chapter.ThumbnailUrl,
                AllowComment = chapter.AllowComment,
                CreatedAt = chapter.CreatedAt,
                ChapterMetaData = new ArtworkChapterMetaData
                {
                    TotalComments = chapter.ChapterMetaData.TotalComments,
                    TotalViews = chapter.ChapterMetaData.TotalViews,
                    TotalFavorites = chapter.ChapterMetaData.TotalFavorites,
                }
            })
            .ToListAsync(cancellationToken);
    }
}
