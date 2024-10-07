using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class G4Repository : IG4Repository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<Artwork> _artworkDbSet;
    private readonly DbSet<ArtworkCategory> _artworkCategoryDbSet;
    private readonly DbSet<Category> _categoryDbSet;

    public G4Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _artworkDbSet = dbContext.Set<Artwork>();
        _artworkCategoryDbSet = dbContext.Set<ArtworkCategory>();
        _categoryDbSet = dbContext.Set<Category>();
    }

    public async Task<List<ArtworkCategory>> GetComicsByCategoryAsync(long CategoryId)
    {
        var result = await _artworkCategoryDbSet
             .Where(c => c.CategoryId == CategoryId)
             .Select(a => new ArtworkCategory
             {
                 Category = new Category
                 {
                     Name = a.Category.Name
                 },
                 Artwork = new Artwork
                 {

                     Id = a.Artwork.Id,
                     Title = a.Artwork.Title,
                     ThumbnailUrl = a.Artwork.ThumbnailUrl,
                     ArtworkMetaData = new ArtworkMetaData
                     {
                         TotalFavorites = a.Artwork.ArtworkMetaData.TotalFavorites,
                         AverageStarRate = a.Artwork.ArtworkMetaData.AverageStarRate
                     }
                 },
             }).AsNoTracking().ToListAsync();
        return result;
    }
}
