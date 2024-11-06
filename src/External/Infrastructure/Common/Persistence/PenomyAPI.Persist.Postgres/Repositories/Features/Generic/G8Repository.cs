using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
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

    public G8Repository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    private static bool IsValidArtworkAsync(Artwork artwork)
    {
        return artwork.ArtworkType == ArtworkType.Comic && artwork.IsTemporarilyRemoved == false && artwork.IsTakenDown == false && artwork.PublicLevel != Domain.RelationalDb.Entities.ArtworkCreation.Common.ArtworkPublicLevel.Private;
    }
    public async Task<(List<ArtworkChapter>, int)> GetArtWorkChapterByIdAsync(
        long id,
        int startPage = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default
    )
    {
        int count = _dbContext.Set<ArtworkChapter>().Count(x => x.ArtworkId == id);
        List<ArtworkChapter> res = await _dbContext
            .Set<ArtworkChapter>()
            .Where(x => x.ArtworkId == id)
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
                    TotalViews = x.ChapterMetaData.TotalViews,
                }
            })
            .AsNoTracking()
            .Skip((startPage - 1) * pageSize)
            .Take(pageSize)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);
        return (res, count);
    }

    public async Task<ArtworkChapterMetaData> GetArtworkChapterMetaDataAsync(
        long id,
        CancellationToken token = default
    )
    {
        return await _dbContext
            .Set<ArtworkChapterMetaData>()
            .Where(x => x.ChapterId == id)
            .Select(x => new ArtworkChapterMetaData
            {
                ChapterId = x.ChapterId,
                TotalComments = x.TotalComments,
                TotalFavorites = x.TotalFavorites,
                TotalViews = x.TotalViews,
            })
            .AsNoTracking()
            .FirstOrDefaultAsync(token);
    }

    public Task<bool> IsArtworkExistAsync(long id, CancellationToken token = default)
    {
        return _dbContext.Set<Artwork>().AnyAsync(x => x.Id == id && x.ArtworkType == ArtworkType.Comic, token);
    }
}
