using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG5;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class G5Repository : IG5Repository
{
    private readonly DbContext _dbContext;

    public G5Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<G5ComicDetailReadModel> GetArtWorkDetailByIdAsync(
        long artworkId,
        CancellationToken token = default
    )
    {
        var artwork = await _dbContext
            .Set<Artwork>()
            .Where(x => x.Id == artworkId)
            .Select(comic => new G5ComicDetailReadModel
            {
                Id = comic.Id,
                Title = comic.Title,
                Introduction = comic.Introduction,
                ThumbnailUrl = comic.ThumbnailUrl,
                AuthorName = comic.AuthorName,
                HasSeries = comic.HasSeries,
                CountryId = comic.Origin.Id,
                CountryName = comic.Origin.CountryName,
                ArtworkCategories = comic.ArtworkCategories.Select(y => new G5CategoryReadModel
                {
                    Id = y.Category.Id,
                    Name = y.Category.Name,
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
                AllowComment = comic.AllowComment,
                // Creator detail section.
                CreatorId = comic.Creator.UserId,
                CreatorName = comic.Creator.NickName,
                CreatorAvatarUrl = comic.Creator.AvatarUrl,
                CreatorTotalFollowers = comic.Creator.CreatorProfile.TotalFollowers,
            })
            .AsNoTracking()
            .AsSplitQuery()
            .FirstOrDefaultAsync(token);

        return artwork;
    }

    public Task<bool> IsComicInUserFavoriteListAsync(
        long userId,
        long artworkId,
        CancellationToken ct = default
    )
    {
        return _dbContext
            .Set<UserFavoriteArtwork>()
            .AnyAsync(x => x.UserId == userId && x.ArtworkId == artworkId, ct);
    }

    public Task<bool> IsArtworkExistAsync(long artworkId, CancellationToken ct = default)
    {
        return _dbContext
            .Set<Artwork>()
            .AnyAsync(x => x.Id == artworkId && x.ArtworkType == ArtworkType.Comic, ct);
    }

    public Task<bool> IsComicInUserFollowedListAsync(
        long userId,
        long artworkId,
        CancellationToken ct = default
    )
    {
        return _dbContext
            .Set<UserFollowedArtwork>()
            .AnyAsync(x => x.UserId == userId && x.ArtworkId == artworkId, ct);
    }
}
