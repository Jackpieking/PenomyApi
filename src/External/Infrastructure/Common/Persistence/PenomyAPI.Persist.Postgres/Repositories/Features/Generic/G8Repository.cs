using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Data.DbContexts;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class G8Repository : IG8Repository
{
    private readonly AppDbContext _dbContext;

    public G8Repository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<ArtworkChapter>> GetArtWorkChapterByIdAsync(
        long id,
        int startPage = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default
    )
    {
        List<ArtworkChapter> res = await _dbContext
            .Set<ArtworkChapter>()
            .Where(x => x.ArtworkId == id)
            .Select(x => new ArtworkChapter
            {
                Id = x.Id,
                BelongedArtwork = new Artwork
                {
                    Id = x.BelongedArtwork.Id,
                    Origin = new ArtworkOrigin
                    {
                        Id = x.BelongedArtwork.Origin.Id,
                        CountryName = x.BelongedArtwork.Origin.CountryName,
                    },
                    ArtworkCategories = x.BelongedArtwork.ArtworkCategories.Select(
                        y => new ArtworkCategory
                        {
                            ArtworkId = y.ArtworkId,
                            CategoryId = y.CategoryId,
                        }
                    ),
                    ArtworkSeries = x.BelongedArtwork.ArtworkSeries.Select(y => new ArtworkSeries
                    {
                        ArtworkId = y.ArtworkId,
                        Series = y.Series,
                    }),
                },
                ArtworkId = x.ArtworkId,
                Title = x.Title,
                PublishedAt = x.PublishedAt,
                CreatedAt = x.CreatedAt,
                UploadOrder = x.UploadOrder,
                ThumbnailUrl = x.ThumbnailUrl,
            })
            .AsNoTracking()
            .Skip((startPage - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        res.ForEach(async x =>
            x.ChapterMetaData = await GetArtworkChapterMetaDataAsync(x.Id, cancellationToken)
        );
        return res;
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
        return _dbContext.Set<ArtworkChapter>().AnyAsync(x => x.ArtworkId == id, token);
    }
}
