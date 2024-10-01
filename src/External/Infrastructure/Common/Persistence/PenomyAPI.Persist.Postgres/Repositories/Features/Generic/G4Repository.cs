using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class G4Repository : IG4Repository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<Artwork> _artworkDbSet;
    private readonly DbSet<ArtworkStatistic> _statisticDbSet;
    private readonly DbSet<ArtworkCategory> _artworkCategoryDbSet;
    private readonly DbSet<Category> _categoryDbSet;

    public G4Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _artworkDbSet = dbContext.Set<Artwork>();
        _artworkCategoryDbSet = dbContext.Set<ArtworkCategory>();
        _categoryDbSet = dbContext.Set<Category>();
    }

    public Task<List<Artwork>> GetComicsByCategoryAsync(string Category)
    {
        var result = _categoryDbSet
             .Where(c => c.Name.Equals(Category))
             .Join(_artworkCategoryDbSet, c => c.Id, ac => ac.CategoryId, (c, ac) => ac.ArtworkId)
             .Join(_artworkDbSet, ac => ac, a => a.Id, (ac, a) => a)
             .Select(a => new Artwork
             {
                 Id = a.Id,
                 Title = a.Title,
                 ThumbnailUrl = a.ThumbnailUrl,
                 Statistic = new ArtworkStatistic
                 {
                     TotalFavorites = a.Statistic.TotalFavorites,
                     AverageStarRate = a.Statistic.AverageStarRate
                 }
             }).ToListAsync();
        return result;
    }
}
