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
            .Select(anime => new G15AnimeDetailReadModel
            {
                Id = anime.Id,
                Title = anime.Title,
                Introduction = anime.Introduction,
                ThumbnailUrl = anime.ThumbnailUrl,
                HasSeries = anime.HasSeries,
                CountryId = anime.Origin.Id,
                CountryName = anime.Origin.CountryName,
                ArtworkCategories = anime.ArtworkCategories.Select(y => new G5CategoryReadModel
                {
                    Id = y.Category.Id,
                    Name = y.Category.Name,
                }),
                ArtworkStatus = anime.ArtworkStatus,
                AllowComment = anime.AllowComment,
                // Creator detail section.
                CreatorId = anime.Creator.UserId,
                TotalChapters = anime.LastChapterUploadOrder,
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
