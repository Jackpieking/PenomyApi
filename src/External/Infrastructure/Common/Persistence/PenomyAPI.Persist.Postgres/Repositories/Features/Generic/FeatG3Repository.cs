using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class FeatG3Repository : IFeatG3Repository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<Artwork> _artworkDbSet;
    private readonly DbSet<ArtworkStatistic> _statisticDbSet;
    private readonly DbSet<ArtworkCategory> _artworkCategoryDbSet;
    private readonly DbSet<Category> _categoryDbSet;

    public FeatG3Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _artworkDbSet = dbContext.Set<Artwork>();
        _statisticDbSet = dbContext.Set<ArtworkStatistic>();
        _artworkCategoryDbSet = dbContext.Set<ArtworkCategory>();
        _categoryDbSet = dbContext.Set<Category>();
    }

    public async Task<List<Artwork>> GetRecentlyUpdatedComicsAsync()
    {
        var result = await _artworkDbSet
            .Select(a => new Artwork()
            {
                Id = a.Id,
                Title = a.Title,
                ThumbnailUrl = a.ThumbnailUrl,
                UpdatedAt = a.UpdatedAt,
                Statistic = new ArtworkStatistic
                {
                    TotalFavorites = a.Statistic.TotalFavorites,
                    AverageStarRate = a.Statistic.AverageStarRate
                }
            })
            .OrderByDescending(a => a.UpdatedAt).Take(20).ToListAsync();
        return result;
    }
}
