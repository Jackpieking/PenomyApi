using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Data.DbContexts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class G6Repository : IG6Repository
{
    private readonly AppDbContext _context;
    private readonly DbSet<Artwork> _artworkDbSet;

    public G6Repository(AppDbContext dbContext)
    {
        _context = dbContext;
        _artworkDbSet = dbContext.Set<Artwork>();
    }

    public async Task<List<Artwork>> GetRecommendedArtworksAsync(int top = 3)
    {
        var list = await _artworkDbSet
            .Select(x => new
            {
                Artwork = new Artwork
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
                },
            })
            .AsNoTracking()
            .AsSplitQuery()
            .ToListAsync();

        return list
            .Select(x => new
            {
                x.Artwork,
                PopularityScore = CalculatePopularityScore(x.Artwork)
            })
            .OrderByDescending(x => x.PopularityScore)
            .Take(top)
            .Select(x => x.Artwork)
            .ToList();
    }


    private double CalculatePopularityScore(Artwork artwork)
    {
        double followWeight = 0.5;
        double favoriteWeight = 0.3;
        double commentWeight = 0.2;

        return (artwork.ArtworkMetaData.TotalFollowers * followWeight)
            + (artwork.ArtworkMetaData.TotalFavorites * favoriteWeight)
            + (artwork.ArtworkMetaData.TotalComments * commentWeight);
    }
}
