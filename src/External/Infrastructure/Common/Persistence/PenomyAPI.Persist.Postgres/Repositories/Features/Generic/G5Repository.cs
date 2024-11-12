using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
            .Select(comic => new Artwork
            {
                Id = comic.Id,
                Title = comic.Title,
                Introduction = comic.Introduction,
                ThumbnailUrl = comic.ThumbnailUrl,
                AuthorName = comic.AuthorName,
                Creator = new UserProfile
                {
                    UserId = comic.Creator.UserId,
                    NickName = comic.Creator.NickName,
                },
                HasSeries = comic.HasSeries,
                Origin = new ArtworkOrigin
                {
                    Id = comic.Origin.Id,
                    CountryName = comic.Origin.CountryName
                },
                ArtworkCategories = comic.ArtworkCategories.Select(y => new ArtworkCategory
                {
                    Category = new Category
                    {
                        Id = y.Category.Id,
                        Name = y.Category.Name
                    },
                }),
                ArtworkStatus = comic.ArtworkStatus,
                ArtworkMetaData = new ArtworkMetaData
                {
                    TotalComments = comic.ArtworkMetaData.TotalComments,
                    TotalFavorites = comic.ArtworkMetaData.TotalFavorites,
                    TotalViews = comic.ArtworkMetaData.TotalViews,
                    TotalStarRates = comic.ArtworkMetaData.TotalStarRates,
                    TotalUsersRated = comic.ArtworkMetaData.TotalUsersRated,
                    AverageStarRate = comic.ArtworkMetaData.AverageStarRate,
                    TotalFollowers = comic.ArtworkMetaData.TotalFollowers
                },
                AllowComment = comic.AllowComment
            })
            .AsNoTracking()
            .AsSplitQuery()
            .FirstOrDefaultAsync(token);

        return artwork;
    }

    public Task<bool> IsComicInUserFavoriteListAsync(
        long userId,
        long artworkId,
        CancellationToken ct = default)
    {
        return _dbContext.Set<UserFavoriteArtwork>()
            .AnyAsync(x => x.UserId == userId && x.ArtworkId == artworkId, ct);
    }

    public Task<bool> IsArtworkExistAsync(long artworkId, CancellationToken ct = default)
    {
        return _dbContext.Set<Artwork>()
            .AnyAsync(x => x.Id == artworkId && x.ArtworkType == ArtworkType.Comic, ct);
    }
}
