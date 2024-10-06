using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class G5Repository : IG5Repository
{
    private readonly DbContext _dbContext;

    public G5Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Artwork> GetArtWorkDetailByIdAsync(
        long artworkId,
        CancellationToken token = default
    )
    {
        var artwork = await _dbContext
            .Set<Artwork>()
            .Where(x => x.Id == artworkId)
            .Select(x => new Artwork
            {
                Id = x.Id,
                Origin = new ArtworkOrigin
                {
                    Id = x.Origin.Id,
                    CountryName = x.Origin.CountryName,
                },
                ArtworkCategories = x.ArtworkCategories.Select(y => new ArtworkCategory
                {
                    ArtworkId = y.ArtworkId,
                    CategoryId = y.CategoryId,
                }),
                ArtworkSeries = x.ArtworkSeries.Select(y => new ArtworkSeries
                {
                    ArtworkId = y.ArtworkId,
                    Series = y.Series,
                }),
                ArtworkStatus = x.ArtworkStatus,
                UserRatingArtworks = x.UserRatingArtworks.Select(y => new UserRatingArtwork
                {
                    StarRates = y.StarRates,
                }),
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
            .FirstOrDefaultAsync(token);
        return artwork;
    }
}