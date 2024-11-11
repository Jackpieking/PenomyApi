using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Data.DbContexts;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class G8Repository : IG8Repository
{
    private readonly AppDbContext _dbContext;
    private readonly DbSet<ArtworkChapter> _chapterDbSet;

    public G8Repository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _chapterDbSet = dbContext.Set<ArtworkChapter>();
    }

    public async Task<List<ArtworkChapter>> GetChapterByComicIdWithPaginationAsync(
        long id,
        int startPage = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default
    )
    {
        return await _chapterDbSet
            .AsNoTracking()
            .Where(chapter => chapter.ArtworkId == id
                && chapter.PublicLevel == ArtworkPublicLevel.Everyone
                && !chapter.IsTemporarilyRemoved
                && chapter.PublishStatus == PublishStatus.Published)
            .Select(x => new ArtworkChapter
            {
                Id = x.Id,
                ArtworkId = x.ArtworkId,
                Title = x.Title,
                PublishedAt = x.PublishedAt,
                CreatedAt = x.CreatedAt,
                UploadOrder = x.UploadOrder,
                ThumbnailUrl = x.ThumbnailUrl,
                ChapterMetaData = new ArtworkChapterMetaData
                {
                    TotalComments = x.ChapterMetaData.TotalComments,
                    TotalFavorites = x.ChapterMetaData.TotalFavorites,
                    TotalViews = x.ChapterMetaData.TotalViews
                },
                AllowComment = x.AllowComment,
            })
            .Skip((startPage - 1) * pageSize)
            .Take(pageSize)
            .OrderBy(x => x.UploadOrder)
            .ToListAsync(cancellationToken);
    }

    public Task<int> GetTotalChaptersByComicIdAsync(
        long comicId,
        CancellationToken cancellationToken)
    {
        return _chapterDbSet.CountAsync(
            chapter => chapter.ArtworkId == comicId
            && chapter.PublicLevel == ArtworkPublicLevel.Everyone
            && !chapter.IsTemporarilyRemoved
            && chapter.PublishStatus == PublishStatus.Published);
    }

    public Task<bool> IsArtworkExistAsync(long id, CancellationToken token = default)
    {
        return _dbContext.Set<Artwork>().AnyAsync(x => x.Id == id, token);
    }
}
