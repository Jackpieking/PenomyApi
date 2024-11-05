using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class G13Repository : IG13Repository
{
    private readonly DbSet<Artwork> _artworkDbSet;

    public G13Repository(DbContext dbContext)
    {
        _artworkDbSet = dbContext.Set<Artwork>();
    }

    public async Task<List<Artwork>> GetRecentlyUpdatedAnimesAsync()
    {
        var result = await _artworkDbSet
            .Where(a =>
                a.ArtworkType == ArtworkType.Animation
                && a.IsTemporarilyRemoved == false
                && a.PublicLevel == ArtworkPublicLevel.Everyone
                && a.IsTakenDown == false
            )
            .Select(a => new Artwork()
            {
                Id = a.Id,
                Title = a.Title,
                ThumbnailUrl = a.ThumbnailUrl,
                UpdatedAt = a.UpdatedAt,
                AuthorName = a.AuthorName,
                Origin = new ArtworkOrigin { ImageUrl = a.Origin.ImageUrl },
                ArtworkMetaData = new ArtworkMetaData
                {
                    TotalFavorites = a.ArtworkMetaData.TotalFavorites,
                    AverageStarRate = a.ArtworkMetaData.AverageStarRate,
                },
            })
            .OrderByDescending(a => a.UpdatedAt)
            .Take(20)
            .AsNoTracking()
            .ToListAsync();
        return result;
    }
}
