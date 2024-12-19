using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG15;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG5;
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

    public Task<G15AnimeDetailReadModel> GetArtWorkDetailByIdAsync(
        long artworkId,
        CancellationToken token = default
    )
    {
        return _dbContext
            .Set<Artwork>()
            .Where(x => x.Id == artworkId)
            .Select(comic => new G15AnimeDetailReadModel
            {
                Id = comic.Id,
                Title = comic.Title,
                Introduction = comic.Introduction,
                ThumbnailUrl = comic.ThumbnailUrl,
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
    }

    public Task<bool> IsArtworkExistAsync(
        long artworkId,
        CancellationToken ct = default)
    {
        return _dbContext
            .Set<Artwork>()
            .AnyAsync(x => x.Id == artworkId && x.ArtworkType == ArtworkType.Animation, ct);
    }
}
