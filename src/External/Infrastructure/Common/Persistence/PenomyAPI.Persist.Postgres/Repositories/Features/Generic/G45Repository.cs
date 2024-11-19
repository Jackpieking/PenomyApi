using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Data.DbContexts;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class G45Repository : IG45Repository
{
    private readonly AppDbContext _dbContext;
    private readonly DbSet<UserFollowedArtwork> _userFolloweds;

    public G45Repository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _userFolloweds = dbContext.Set<UserFollowedArtwork>();
    }

    public async Task<ICollection<Artwork>> GetFollowedArtworksByTypeAndUserIdWithPaginationAsync(long userId, ArtworkType artworkType, int pageNum, int artNum, CancellationToken ct)
    {
        return await _userFolloweds.AsNoTracking()
            .Where(o => o.UserId == userId && o.ArtworkType == artworkType && !o.FollowedArtwork.IsTakenDown)
            .OrderByDescending(o => o.StartedAt)
            .Skip((pageNum - 1) * artNum)
            .Take(artNum)
            .Select(o => new Artwork
            {
                Id = o.ArtworkId,
                Title = o.FollowedArtwork.Title,
                CreatedBy = o.FollowedArtwork.CreatedBy,
                AuthorName = o.FollowedArtwork.AuthorName,
                ThumbnailUrl = o.FollowedArtwork.ThumbnailUrl,
                PublicLevel = o.FollowedArtwork.PublicLevel,
                ArtworkMetaData = new ArtworkMetaData
                {
                    TotalFavorites = o.FollowedArtwork.ArtworkMetaData.TotalFavorites,
                    AverageStarRate = o.FollowedArtwork.ArtworkMetaData.GetAverageStarRate()
                },
                Origin = new ArtworkOrigin
                {
                    ImageUrl = o.FollowedArtwork.Origin.ImageUrl
                },
                Chapters = o.FollowedArtwork.Chapters
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

    public async Task<int> GetTotalOfArtworksByTypeAndUserIdAsync(long userId, ArtworkType artworkType, CancellationToken cancellationToken)
    {
        return await _userFolloweds.AsNoTracking()
            .CountAsync(o =>
                o.UserId == userId &&
                o.ArtworkType == artworkType &&
                !o.FollowedArtwork.IsTakenDown,
                cancellationToken: cancellationToken);
    }
}
