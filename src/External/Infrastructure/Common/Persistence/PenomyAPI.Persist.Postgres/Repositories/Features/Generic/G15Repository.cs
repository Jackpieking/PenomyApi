using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class G15Repository : IG15Repository
{
    private readonly DbContext _dbContext;

    public G15Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }
    private static bool IsValidArtworkAsync(Artwork artwork)
    {
        return artwork.ArtworkType == ArtworkType.Animation && artwork.IsTemporarilyRemoved == false && artwork.IsTakenDown == false && artwork.PublicLevel != Domain.RelationalDb.Entities.ArtworkCreation.Common.ArtworkPublicLevel.Private;
    }
    public async Task<Artwork> GetArtWorkDetailByIdAsync(
        long artworkId,
        CancellationToken token = default
    )
    {
        var artwork = await _dbContext
            .Set<Artwork>()
            .Where(x => x.Id == artworkId && IsValidArtworkAsync(x))
            .Select(x => new Artwork
            {
                Title = x.Title,
                AuthorName = x.AuthorName,
                HasSeries = x.HasSeries,
                Introduction = x.Introduction,
                Id = x.Id,
                Origin = new ArtworkOrigin
                {
                    Id = x.Origin.Id,
                    CountryName = x.Origin.CountryName,
                },
                ArtworkCategories = x.ArtworkCategories.Select(y => new ArtworkCategory
                {
                    Category = new Category { Name = y.Category.Name, },
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
            .AsNoTracking()
            .AsSplitQuery()
            .FirstOrDefaultAsync(token);
        return artwork;
    }

    public Task<bool> IsArtworkExistAsync(long artworkId, CancellationToken ct = default)
    {
        return _dbContext.Set<Artwork>().AnyAsync(x => x.Id == artworkId && x.ArtworkType == ArtworkType.Animation, ct);
    }
}
