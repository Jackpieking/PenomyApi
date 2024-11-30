using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG48;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Data.DbContexts;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class G48Repository : IG48Repository
{
    private const int TOTAL_NUMBERS_OF_ARTWORKS = 64;
    private readonly DbContext _dbContext;
    private readonly DbSet<UserFavoriteArtwork> _userFavoriteArtworks;

    public G48Repository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
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

    public async Task<List<G48FavoriteArtworkReadModel>> GetFavoriteArtworksByTypeAndUserIdWithPaginationAsync(
        long userId,
        ArtworkType artworkType,
        int pageNum,
        int artNum,
        CancellationToken ct)
    {
        var followedArtworks = await _userFavoriteArtworks
            .AsNoTracking()
            .Where(o => o.UserId == userId && o.ArtworkType == artworkType)
            .OrderByDescending(o => o.StartedAt)
            .Select(o => new G48FavoriteArtworkReadModel
            {
                Id = o.ArtworkId,
                Title = o.FavoriteArtwork.Title,
                ThumbnailUrl = o.FavoriteArtwork.ThumbnailUrl,
                ArtworkStatus = o.FavoriteArtwork.ArtworkStatus,
                OriginImageUrl = o.FavoriteArtwork.Origin.ImageUrl,
                LastChapterUploadOrder = o.FavoriteArtwork.LastChapterUploadOrder,
                TotalStarRates = o.FavoriteArtwork.ArtworkMetaData.TotalStarRates,
                TotalUsersRated = o.FavoriteArtwork.ArtworkMetaData.TotalUsersRated,
                // Creator detail section.
                CreatorId = o.FavoriteArtwork.Creator.UserId,
                CreatorName = o.FavoriteArtwork.Creator.NickName,
                CreatorAvatarUrl = o.FavoriteArtwork.Creator.AvatarUrl,
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
                .Select(chapter => new G48ChapterReadModel
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
}
