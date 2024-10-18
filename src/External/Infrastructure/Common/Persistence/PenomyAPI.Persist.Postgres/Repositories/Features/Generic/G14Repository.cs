using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Data.DbContexts;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class G14Repository : IG14Repository
{
    private readonly AppDbContext _dbContext;
    private readonly DbSet<Artwork> _artworkSet;
    public G14Repository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _artworkSet = _dbContext.Set<Artwork>();
    }

    public async Task<List<Artwork>> GetRecommendedAnimeAsync(long cateId, int limit = 5, CancellationToken cancellationToken = default)
    {
        var result = await _artworkSet
            .Where(x => x.ArtworkType == ArtworkType.Animation && x.ArtworkCategories.Any(y => y.CategoryId == cateId))
            .Take(limit)
            .Select(x => new Artwork
            {
                Title = x.Title,
                AuthorName = x.AuthorName,
                Introduction = x.Introduction,
                Id = x.Id,
                Origin = new ArtworkOrigin
                {
                    Id = x.Origin.Id,
                    CountryName = x.Origin.CountryName,
                },
                ArtworkCategories = x.ArtworkCategories.Select(y => new ArtworkCategory
                {
                    Category = new Category { Name = y.Category.Name },
                    ArtworkId = y.ArtworkId,
                    CategoryId = y.CategoryId,
                }).ToList(),
                ArtworkSeries = x.ArtworkSeries.Select(y => new ArtworkSeries
                {
                    ArtworkId = y.ArtworkId,
                    Series = y.Series,
                }).ToList(),
                ArtworkStatus = x.ArtworkStatus,
                UserRatingArtworks = x.UserRatingArtworks.Select(y => new UserRatingArtwork
                {
                    StarRates = y.StarRates,
                }).ToList(),
                ArtworkMetaData = new ArtworkMetaData
                {
                    TotalComments = x.ArtworkMetaData.TotalComments,
                    TotalFavorites = x.ArtworkMetaData.TotalFavorites,
                    TotalViews = x.ArtworkMetaData.TotalViews,
                    TotalStarRates = x.ArtworkMetaData.TotalStarRates,
                    TotalUsersRated = x.ArtworkMetaData.TotalUsersRated,
                    AverageStarRate = x.ArtworkMetaData.AverageStarRate,
                },
                ThumbnailUrl = x.ThumbnailUrl,
            })
            .OrderByDescending(x => x.ArtworkMetaData.TotalStarRates)
            .AsNoTracking()
            .AsSplitQuery()
            .ToListAsync(cancellationToken);

        return result;
    }

    public async Task<bool> IsExistCategoryAsync(long cateId, CancellationToken token = default)
    {
        return await _dbContext.Set<Category>().AnyAsync(x => x.Id == cateId, token);
    }
}
