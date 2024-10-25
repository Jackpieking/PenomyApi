using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.ArtworkCreation;

internal sealed class Art5Repository : IArt5Repository
{
    private readonly DbContext _dbContext;

    public Art5Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Artwork> GetComicDetailByIdAsync(
        long comicId,
        CancellationToken cancellationToken)
    {
        var comicDetail = await _dbContext.Set<Artwork>()
            .AsNoTracking()
            .Where(comic => comic.Id == comicId)
            .Select(comic => new Artwork
            {
                Id = comic.Id,
                Title = comic.Title,
                ThumbnailUrl = comic.ThumbnailUrl,
                Introduction = comic.Introduction,
                Origin = new ArtworkOrigin
                {
                    CountryName = comic.Origin.CountryName
                },
                ArtworkStatus = comic.ArtworkStatus,
                Creator = new UserProfile
                {
                    NickName = comic.Creator.NickName,
                },
                HasSeries = comic.HasSeries,
                ArtworkCategories = comic.ArtworkCategories.Select(artworkCategory => new ArtworkCategory
                {
                    Category = new Category
                    {
                        Id = artworkCategory.CategoryId,
                        Name = artworkCategory.Category.Name,
                    }
                })
            })
            .AsSplitQuery()
            .FirstOrDefaultAsync(cancellationToken);

        // Check if the current comic has series or not to fetch the data
        if (comicDetail.HasSeries)
        {
            var artworkSeries = await _dbContext.Set<ArtworkSeries>()
                .AsNoTracking()
                .Where(artworkSeries => artworkSeries.ArtworkId == comicId)
                .Select(artworkSeries => new ArtworkSeries
                {
                    Series = new Series
                    {
                        Id = artworkSeries.SeriesId,
                        Title = artworkSeries.Series.Title
                    }
                })
                .Take(1)
                .ToListAsync(cancellationToken);

            comicDetail.ArtworkSeries = artworkSeries;
        }

        return comicDetail;
    }
}
