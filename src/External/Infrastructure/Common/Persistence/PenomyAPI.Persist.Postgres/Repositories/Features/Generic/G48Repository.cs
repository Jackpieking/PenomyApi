using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Data.DbContexts;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class G48Repository : IG48Repository
{
    private readonly DbSet<UserFavoriteArtwork> _userFavoriteArtworks;

    public G48Repository(AppDbContext dbContext)
    {
        _userFavoriteArtworks = dbContext.Set<UserFavoriteArtwork>();
    }

    public Task<int> GetTotalOfArtworksByTypeAndUserIdAsync(
        long userId,
        ArtworkType artworkType,
        CancellationToken cancellationToken)
    {
        return _userFavoriteArtworks.CountAsync(
            predicate: o =>
                o.UserId == userId
                && o.FavoriteArtwork.ArtworkType == artworkType
                && !o.FavoriteArtwork.IsTemporarilyRemoved,
            cancellationToken: cancellationToken);
    }

    public async Task<ICollection<Artwork>> GetFavoriteArtworksByTypeAndUserIdWithPaginationAsync(
        long userId,
        ArtworkType artworkType,
        int pageNum,
        int artNum,
        CancellationToken ct)
    {
        return await _userFavoriteArtworks.AsNoTracking()
            .Where(o =>
                o.UserId == userId &&
                o.ArtworkType == artworkType &&
                !o.FavoriteArtwork.IsTakenDown &&
                !o.FavoriteArtwork.IsTemporarilyRemoved)
            .OrderByDescending(o => o.StartedAt)
            .Skip((pageNum - 1) * artNum)
            .Take(artNum)
            .Select(o => new Artwork
            {
                Id = o.ArtworkId,
                Title = o.FavoriteArtwork.Title,
                CreatedBy = o.FavoriteArtwork.CreatedBy,
                AuthorName = o.FavoriteArtwork.AuthorName,
                ThumbnailUrl = o.FavoriteArtwork.ThumbnailUrl,
                PublicLevel = o.FavoriteArtwork.PublicLevel,
                ArtworkMetaData = new ArtworkMetaData
                {
                    TotalFavorites = o.FavoriteArtwork.ArtworkMetaData.TotalFavorites,
                    AverageStarRate = o.FavoriteArtwork.ArtworkMetaData.GetAverageStarRate()
                },
                Origin = new ArtworkOrigin
                {
                    ImageUrl = o.FavoriteArtwork.Origin.ImageUrl
                },
                Chapters = o.FavoriteArtwork.Chapters
                    .OrderByDescending(c => c.UploadOrder)
                    .Take(2)
                    .Select(c => new ArtworkChapter
                    {
                        Id = c.Id,
                        Title = c.Title,
                        UploadOrder = c.UploadOrder,
                        UpdatedAt = c.UpdatedAt
                    })
            })
            .ToListAsync();
    }
}
