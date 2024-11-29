using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG45;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Data.DbContexts;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class G45Repository : IG45Repository
{
    private const int TOTAL_NUMBERS_OF_ARTWORKS = 64;
    private readonly AppDbContext _dbContext;
    private readonly DbSet<UserFollowedArtwork> _userFolloweds;

    public G45Repository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _userFolloweds = dbContext.Set<UserFollowedArtwork>();
    }

    public async Task<List<G45FollowedArtworkReadModel>> GetFollowedArtworksByTypeAndUserIdWithPaginationAsync(long userId, ArtworkType artworkType, int pageNum, int artNum, CancellationToken ct)
    {
        var followedArtworks = await _userFolloweds
            .AsNoTracking()
            .Where(o => o.UserId == userId && o.ArtworkType == artworkType)
            .OrderByDescending(o => o.StartedAt)
            .Select(o => new G45FollowedArtworkReadModel
            {
                Id = o.ArtworkId,
                Title = o.FollowedArtwork.Title,
                ThumbnailUrl = o.FollowedArtwork.ThumbnailUrl,
                ArtworkStatus = o.FollowedArtwork.ArtworkStatus,
                OriginImageUrl = o.FollowedArtwork.Origin.ImageUrl,
                LastChapterUploadOrder = o.FollowedArtwork.LastChapterUploadOrder,
                TotalStarRates = o.FollowedArtwork.ArtworkMetaData.TotalStarRates,
                TotalUsersRated = o.FollowedArtwork.ArtworkMetaData.TotalUsersRated,
                // Creator detail section.
                CreatorId = o.FollowedArtwork.Creator.UserId,
                CreatorName = o.FollowedArtwork.Creator.NickName,
                CreatorAvatarUrl = o.FollowedArtwork.Creator.AvatarUrl,
            })
            .Take(TOTAL_NUMBERS_OF_ARTWORKS)
            .ToListAsync(ct);


        // Get latest chapter of each artwork.
        var chapterDbSet = _dbContext.Set<ArtworkChapter>();

        foreach (var item in followedArtworks)
        {
            var lastestChapter = await chapterDbSet
                .AsNoTracking()
                .Where(
                    chapter => chapter.ArtworkId == item.Id
                    && chapter.PublicLevel == ArtworkPublicLevel.Everyone
                    && chapter.PublishStatus == PublishStatus.Published
                    && (chapter.UploadOrder == item.LastChapterUploadOrder || chapter.UploadOrder <= item.LastChapterUploadOrder))
                .OrderByDescending(chapter => chapter.UploadOrder)
                .Select(chapter => new G45ChapterReadModel
                {
                    Id = chapter.Id,
                    UploadOrder = chapter.UploadOrder,
                    PublishedAt = chapter.PublishedAt
                })
                .FirstOrDefaultAsync(ct);

            item.Chapter = lastestChapter;
        }

        return followedArtworks;
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
