using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class G12Repository : IG12Repository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<Artwork> _artworkDbSet;
    private readonly DbSet<ArtworkCategory> _artworkCategoryDbSet;
    private readonly DbSet<Category> _categoryDbSet;

    public G12Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _artworkDbSet = dbContext.Set<Artwork>();
        _artworkCategoryDbSet = dbContext.Set<ArtworkCategory>();
        _categoryDbSet = dbContext.Set<Category>();
    }

    public async Task<List<ArtworkCategory>> GetAnimesByCategoryAsync(long CategoryId)
    {
        var result = await _artworkCategoryDbSet
            .Where(c =>
                c.CategoryId == CategoryId && c.Artwork.ArtworkType == ArtworkType.Animation
            )
            .Select(a => new ArtworkCategory
            {
                Category = new Category { Name = a.Category.Name },
                Artwork = new Artwork
                {
                    Id = a.Artwork.Id,
                    Title = a.Artwork.Title,
                    ThumbnailUrl = a.Artwork.ThumbnailUrl,
                    Origin = new ArtworkOrigin { ImageUrl = a.Artwork.Origin.ImageUrl },
                    ArtworkMetaData = new ArtworkMetaData
                    {
                        TotalFavorites = a.Artwork.ArtworkMetaData.TotalFavorites,
                        AverageStarRate = a.Artwork.ArtworkMetaData.AverageStarRate,
                        TotalViews = a.Artwork.ArtworkMetaData.TotalViews,
                    },
                },
            })
            .AsNoTracking()
             .OrderByDescending(ac => ac.Artwork.ArtworkMetaData.TotalViews)
             .ThenByDescending(ac => ac.Artwork.ArtworkMetaData.AverageStarRate)
            .Take(23)
            .ToListAsync();
        return result;
    }
}
