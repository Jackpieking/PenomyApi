using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG19;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class G19Repository : IG19Repository
{
    private const int MAX_NUMBER_OF_RETURN_CHAPTERS = 100;
    private readonly DbContext _dbContext;
    private readonly DbSet<ArtworkChapter> _chapterDbSet;

    public G19Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _chapterDbSet = dbContext.Set<ArtworkChapter>();
    }

    public Task<List<G19AnimeChapterItemReadModel>> GetAllChaptersAsyncByAnimeId(
        long animeId,
        CancellationToken cancellationToken)
    {
        return _chapterDbSet
            .AsNoTracking()
            .Where(chapter => chapter.ArtworkId == animeId
                && chapter.PublicLevel == ArtworkPublicLevel.Everyone
                && !chapter.IsTemporarilyRemoved
                && chapter.PublishStatus == PublishStatus.Published)
            .Select(chapter => new G19AnimeChapterItemReadModel
            {
                Id = chapter.Id,
                ChapterName = chapter.Title,
                ThumbnailUrl = chapter.ThumbnailUrl,
                UploadOrder = chapter.UploadOrder,
            })
            .OrderBy(chapter => chapter.UploadOrder)
            .Take(MAX_NUMBER_OF_RETURN_CHAPTERS)
            .ToListAsync(cancellationToken); ;
    }

    public Task<G19AnimeChapterDetailReadModel> GetChapterDetailByIdAsync(
        long chapterId,
        CancellationToken cancellationToken)
    {
        return _chapterDbSet
            .AsNoTracking()
            .Where(chapter => chapter.Id == chapterId)
            .Select(chapter => new G19AnimeChapterDetailReadModel
            {
                Id = chapter.Id,
                ArtworkId = chapter.ArtworkId,
                Title = chapter.Title,
                Description = chapter.Description,
                UploadOrder = chapter.UploadOrder,
                AllowComment = chapter.AllowComment,
                CreatedBy = chapter.CreatedBy,
                ChapterVideoUrl = chapter.ChapterMedias.FirstOrDefault().StorageUrl,
                TotalViews = chapter.ChapterMetaData.TotalViews,
            })
            .AsSplitQuery()
            .FirstOrDefaultAsync(cancellationToken);
    }
}
