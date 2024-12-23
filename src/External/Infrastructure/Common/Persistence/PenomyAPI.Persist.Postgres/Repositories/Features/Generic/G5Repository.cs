using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG5;
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

    public async Task<G5FirstAndLastReadChapterReadModel> GetFirstAndLastReadChapterOfArtworkForGuestAsync(
        long guestId,
        long artworkId,
        CancellationToken ct
    )
    {
        const int FIRST_CHAPTER_UPLOAD_ORDER = 1;

        var chapterDbSet = _dbContext.Set<ArtworkChapter>();

        long firstComicChapterId = await chapterDbSet
            .Where(chapter =>
                chapter.ArtworkId == artworkId
                && !chapter.IsTemporarilyRemoved
                && chapter.PublicLevel == ArtworkPublicLevel.Everyone
                && chapter.UploadOrder == FIRST_CHAPTER_UPLOAD_ORDER
            )
            .Select(chapter => chapter.Id)
            .FirstOrDefaultAsync(ct);

        // Get last read chapter id from the guest view history.
        var guestViewHistoryDbSet = _dbContext.Set<GuestArtworkViewHistory>();

        long lastReadChapterId = await guestViewHistoryDbSet
            .Where(
                viewHistory => viewHistory.GuestId == guestId
                && viewHistory.ArtworkId == artworkId
            )
            .Select(chapter => chapter.ChapterId)
            .FirstOrDefaultAsync(ct);

        if (lastReadChapterId == default)
        {
            lastReadChapterId = firstComicChapterId;
        }

        return new G5FirstAndLastReadChapterReadModel
        {
            FirstChapterId = firstComicChapterId,
            LastReadChapterId = lastReadChapterId
        };
    }

    public async Task<G5FirstAndLastReadChapterReadModel> GetFirstAndLastReadChapterOfArtworkForUserAsync(
        long userId,
        long artworkId,
        CancellationToken ct
    )
    {
        const int FIRST_CHAPTER_UPLOAD_ORDER = 1;

        var chapterDbSet = _dbContext.Set<ArtworkChapter>();

        long firstComicChapterId = await chapterDbSet
            .Where(chapter =>
                chapter.ArtworkId == artworkId
                && !chapter.IsTemporarilyRemoved
                && chapter.PublicLevel == ArtworkPublicLevel.Everyone
                && chapter.UploadOrder == FIRST_CHAPTER_UPLOAD_ORDER
            )
            .Select(chapter => chapter.Id)
            .FirstOrDefaultAsync(ct);

        // Get last read chapter id from the user view history.
        var guestViewHistoryDbSet = _dbContext.Set<UserArtworkViewHistory>();

        long lastReadChapterId = await guestViewHistoryDbSet
            .Where(
                viewHistory => viewHistory.UserId == userId
                && viewHistory.ArtworkId == artworkId
            )
            .Select(chapter => chapter.ChapterId)
            .FirstOrDefaultAsync(ct);

        if (lastReadChapterId == default)
        {
            lastReadChapterId = firstComicChapterId;
        }

        return new G5FirstAndLastReadChapterReadModel
        {
            FirstChapterId = firstComicChapterId,
            LastReadChapterId = lastReadChapterId
        };
    }

    public async Task<G5UserArtworkPreferenceReadModel> GetUserArtworkPreferenceAsync(
        long userId,
        long artworkId,
        CancellationToken cancellationToken)
    {
        var isInFavoriteList = await IsComicInUserFavoriteListAsync(
            userId: userId,
            artworkId: artworkId,
            ct: cancellationToken);

        var isInFollowedList = await IsComicInUserFollowedListAsync(
            userId: userId,
            artworkId: artworkId,
            ct: cancellationToken);

        var firstAndLastReadChapter = await GetFirstAndLastReadChapterOfArtworkForUserAsync(
            userId: userId,
            artworkId: artworkId,
            ct: cancellationToken);

        return new()
        {
            HasFollowed = isInFollowedList,
            IsUserFavorite = isInFavoriteList,
            FirstChapterId = firstAndLastReadChapter.FirstChapterId,
            LastReadChapterId = firstAndLastReadChapter.LastReadChapterId,
        };
    }

    public async Task<G5UserArtworkPreferenceReadModel> GetGuestArtworkPreferenceAsync(
        long guestId,
        long artworkId,
        CancellationToken cancellationToken)
    {
        var firstAndLastReadChapter = await GetFirstAndLastReadChapterOfArtworkForGuestAsync(
            guestId: guestId,
            artworkId: artworkId,
            ct: cancellationToken);

        return new()
        {
            HasFollowed = false,
            IsUserFavorite = false,
            FirstChapterId = firstAndLastReadChapter.FirstChapterId,
            LastReadChapterId = firstAndLastReadChapter.LastReadChapterId,
        };
    }

    public Task<G5CreatorProfileReadModel> GetCreatorProfileBasedOnUserIdAsync(
        long userId,
        long creatorId,
        CancellationToken cancellationToken)
    {
        return _dbContext.Set<UserProfile>()
            .AsNoTracking()
            .Where(profile => profile.UserId == creatorId)
            .Select(profile => new G5CreatorProfileReadModel
            {
                CreatorId = creatorId,
                CreatorAvatarUrl = profile.AvatarUrl,
                CreatorName = profile.NickName,
                CreatorTotalFollowers = profile.CreatorProfile.TotalFollowers,
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<bool> IsCreatorProfileExistedAsync(
        long creatorId,
        CancellationToken cancellationToken)
    {
        return _dbContext.Set<CreatorProfile>().AnyAsync(
            profile => profile.CreatorId == creatorId,
            cancellationToken);
    }

    public Task<ArtworkMetaData> GetArtworkMetaDataByIdAsync(
        long artworkId,
        CancellationToken cancellationToken)
    {
        return _dbContext.Set<ArtworkMetaData>()
            .AsNoTracking()
            .Where(artwork => artwork.ArtworkId == artworkId)
            .Select(metadata => new ArtworkMetaData
            {
                ArtworkId = artworkId,
                TotalViews = metadata.TotalViews,
                TotalComments = metadata.TotalComments,
                TotalFavorites = metadata.TotalFavorites,
                TotalStarRates = metadata.TotalStarRates,
                TotalFollowers = metadata.TotalFollowers,
                TotalUsersRated = metadata.TotalUsersRated,
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}
