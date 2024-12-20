using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG9;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

internal sealed class G9Repository : IG9Repository
{
    private const int MAX_NUMBER_OF_RETURN_CHAPTERS = 100;
    private readonly DbContext _dbContext;
    private readonly DbSet<ArtworkChapter> _chapterDbSet;

    public G9Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _chapterDbSet = dbContext.Set<ArtworkChapter>();
    }

    public Task<List<G9ChapterItemReadModel>> GetAllChaptersAsyncByComicId(
        long comicId,
        CancellationToken cancellationToken)
    {
        return _chapterDbSet
            .AsNoTracking()
            .Where(chapter => chapter.ArtworkId == comicId
                && chapter.PublicLevel == ArtworkPublicLevel.Everyone
                && !chapter.IsTemporarilyRemoved
                && chapter.PublishStatus == PublishStatus.Published)
            .Select(chapter => new G9ChapterItemReadModel
            {
                Id = chapter.Id,
                ChapterName = chapter.Title,
                ThumbnailUrl = chapter.ThumbnailUrl,
                UploadOrder = chapter.UploadOrder,
            })
            .OrderBy(chapter => chapter.UploadOrder)
            .Take(MAX_NUMBER_OF_RETURN_CHAPTERS)
            .ToListAsync(cancellationToken);
    }

    public Task<ArtworkChapter> GetComicChapterDetailByIdAsync(
        long comicId,
        long chapterId,
        CancellationToken cancellationToken)
    {
        return _chapterDbSet
            .AsNoTracking()
            .Where(chapter =>
                chapter.ArtworkId == comicId
                && chapter.Id == chapterId)
            .Select(chapter => new ArtworkChapter
            {
                Id = chapter.Id,
                ArtworkId = chapter.ArtworkId,
                Title = chapter.Title,
                Description = chapter.Description,
                BelongedArtwork = new Artwork
                {
                    Title = chapter.BelongedArtwork.Title,
                },
                UploadOrder = chapter.UploadOrder,
                AllowComment = chapter.AllowComment,
                CreatedBy = chapter.CreatedBy,
                ChapterMetaData = new ArtworkChapterMetaData
                {
                    TotalFavorites = chapter.ChapterMetaData.TotalFavorites,
                },
                ChapterMedias = chapter.ChapterMedias.Select(media => new ArtworkChapterMedia
                {
                    UploadOrder = media.UploadOrder,
                    StorageUrl = media.StorageUrl,
                })
            })
            .AsSplitQuery()
            .FirstOrDefaultAsync(cancellationToken);
    }
}
