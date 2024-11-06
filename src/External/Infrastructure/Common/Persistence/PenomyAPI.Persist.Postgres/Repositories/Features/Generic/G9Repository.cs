using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Domain.RelationalDb.Models.Generic.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

internal sealed class G9Repository : IG9Repository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<ArtworkChapter> _chapterDbSet;

    public G9Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _chapterDbSet = dbContext.Set<ArtworkChapter>();
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

    public async Task<PreviousAndNextChapter> GetPreviousAndNextChapterOfCurrentChapterAsync(
        long comicId,
        int currentChapterOrder,
        CancellationToken cancellationToken)
    {
        var prevAndNextChapter = await _chapterDbSet
            .AsNoTracking()
            .Where(
                chapter => chapter.ArtworkId == comicId
                && chapter.PublishStatus == PublishStatus.Published
                && (chapter.UploadOrder == currentChapterOrder - 1
                    || chapter.UploadOrder == currentChapterOrder + 1))
            .OrderBy(chapter => chapter.UploadOrder)
            .Select(chapter => chapter.Id)
            .ToArrayAsync(cancellationToken);

        // If current chapter order is the first, then the array will contain only the next chapter.
        long prevChapterId = default;
        long nextChapterId = default;

        if (currentChapterOrder == 1)
        {
            nextChapterId = prevAndNextChapter[0];

            return new()
            {
                PrevChapterId = -1,
                NextChapterId = nextChapterId,
            };
        }

        // If the current chapter order is different than 1
        // but the array return still has length = 1.
        // Then the current chapter order must be the last order.
        if (prevAndNextChapter.Length == 1)
        {
            prevChapterId = prevAndNextChapter[0];

            return new()
            {
                PrevChapterId = prevChapterId,
                NextChapterId = -1,
            };
        }

        // Otherwise return the prev and next.
        prevChapterId = prevAndNextChapter[0];
        nextChapterId = prevAndNextChapter[prevAndNextChapter.Length - 1];

        return new()
        {
            PrevChapterId = prevChapterId,
            NextChapterId = nextChapterId,
        };
    }
}
