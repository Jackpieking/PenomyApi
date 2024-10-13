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
    private readonly DbSet<ArtworkMetaData> _statisticDbSet;

    public FeatG3Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _artworkDbSet = dbContext.Set<Artwork>();
        _statisticDbSet = dbContext.Set<ArtworkMetaData>();
    }

    public async Task<List<Artwork>> GetRecentlyUpdatedComicsAsync()
    {
        var result = await _artworkDbSet
            .Where(a => a.ArtworkType == ArtworkType.Comic && a.IsTemporarilyRemoved == false)
            .Select(a => new Artwork()
            {
                Id = a.Id,
                Title = a.Title,
                ThumbnailUrl = a.ThumbnailUrl,
                UpdatedAt = a.UpdatedAt,
                AuthorName = a.AuthorName,
                Origin = new ArtworkOrigin
                {
                    ImageUrl = a.Origin.ImageUrl
                },
                ArtworkMetaData = new ArtworkMetaData
                {
                    TotalFavorites = a.ArtworkMetaData.TotalFavorites,
                    AverageStarRate = a.ArtworkMetaData.AverageStarRate
                }
            })
            .OrderByDescending(a => a.UpdatedAt)
            .Take(20)
            .AsNoTracking()
            .ToListAsync();
        return result;
    }
}
