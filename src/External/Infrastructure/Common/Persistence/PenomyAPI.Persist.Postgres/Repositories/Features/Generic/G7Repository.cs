using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Data.DbContexts;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic
{
    public class G7Repository : IG7Repository
    {
        private readonly AppDbContext _context;

        public G7Repository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Artwork>> GetArkworkBySeriesAsync(
            long currentArkworkId,
            int startPage = 1,
            int pageSize = 3,
            CancellationToken cancellationToken = default
        )
        {
            List<Artwork> artworks = new();
            if (startPage < 1)
            {
                startPage = 1;
            }
            long seriesId = await _context
                .Set<ArtworkSeries>()
                .Where(x => x.ArtworkId == currentArkworkId)
                .Select(x => x.SeriesId)
                .FirstOrDefaultAsync();

            if (seriesId > 0)
            {
                artworks = await _context
                    .Set<Artwork>()
                    .Where(x => x.ArtworkSeries.Any(x => x.SeriesId == seriesId))
                    .Select(x => new Artwork()
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
                    })
                    .AsNoTracking()
                    .OrderBy(x => x.Id)
                    .Skip((startPage - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(cancellationToken);
                artworks.ForEach(async x =>
                {
                    x.ArtworkMetaData = await GetArtworkMetaDataAsync(x.Id, cancellationToken);
                });
            }
            return artworks;
        }

        public async Task<ArtworkMetaData> GetArtworkMetaDataAsync(
            long artworkId,
            CancellationToken token = default
        )
        {
            return await _context
                .Set<ArtworkMetaData>()
                .Where(x => x.ArtworkId == artworkId)
                .Select(x => new ArtworkMetaData
                {
                    ArtworkId = x.ArtworkId,
                    TotalComments = x.TotalComments,
                    TotalFavorites = x.TotalFavorites,
                    TotalViews = x.TotalViews,
                    TotalStarRates = x.TotalStarRates,
                    TotalUsersRated = x.TotalUsersRated,
                    AverageStarRate = x.AverageStarRate,
                })
                .FirstOrDefaultAsync(token);
        }

        public Task<bool> IsArtworkExistAsync(long artworkId, CancellationToken token = default)
        {
            return _context.Set<Artwork>().AnyAsync(x => x.Id == artworkId, token);
        }
    }
}
