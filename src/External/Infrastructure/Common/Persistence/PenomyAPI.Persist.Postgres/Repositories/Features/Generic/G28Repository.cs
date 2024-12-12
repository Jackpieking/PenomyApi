using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG28;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class G28Repository : IG28Repository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<Artwork> _artworkDbSet;
    private readonly DbSet<Series> _seriesDbSet;

    public G28Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _artworkDbSet = dbContext.Set<Artwork>();
        _seriesDbSet = dbContext.Set<Series>();
    }

    public Task<int> GetPaginationOptionsByArtworkTypeAsync(
        ArtworkType artworkType,
        long creatorId
    )
    {
        return _artworkDbSet
            .Where(o =>
                o.CreatedBy == creatorId
                && o.ArtworkType == artworkType
                && !o.IsTemporarilyRemoved
                && !o.IsTakenDown
                && o.PublicLevel == ArtworkPublicLevel.Everyone
            )
            .CountAsync();
    }

    public async Task<List<G28ArtworkDetailReadModel>> GetPaginationDetailAsync(
        long creatorId,
        ArtworkType artworkType,
        int pageNumber,
        int pageSize
    )
    {
        var followedArtworks = await _artworkDbSet
            .AsNoTracking()
            .Where(o =>
                o.CreatedBy == creatorId
                && o.ArtworkType == artworkType
                && !o.IsTemporarilyRemoved
                && !o.IsTakenDown
                && o.PublicLevel == ArtworkPublicLevel.Everyone
            )
            .OrderByDescending(o => o.CreatedAt)
            .Select(o => new G28ArtworkDetailReadModel
            {
                Id = o.Id,
                Title = o.Title,
                ThumbnailUrl = o.ThumbnailUrl,
                ArtworkStatus = o.ArtworkStatus,
                OriginImageUrl = o.Origin.ImageUrl,
                LastChapterUploadOrder = o.LastChapterUploadOrder,
                TotalStarRates = o.ArtworkMetaData.TotalStarRates,
                TotalUsersRated = o.ArtworkMetaData.TotalUsersRated,
            })
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        // Get latest chapter of each artwork.
        var chapterDbSet = _dbContext.Set<ArtworkChapter>();

        foreach (var item in followedArtworks)
        {
            var lastestChapter = await chapterDbSet
                .AsNoTracking()
                .Where(chapter =>
                    chapter.ArtworkId == item.Id
                    && chapter.PublicLevel == ArtworkPublicLevel.Everyone
                    && chapter.PublishStatus == PublishStatus.Published
                    && (
                        chapter.UploadOrder == item.LastChapterUploadOrder
                        || chapter.UploadOrder <= item.LastChapterUploadOrder
                    )
                )
                .OrderByDescending(chapter => chapter.UploadOrder)
                .Select(chapter => new G28ChapterReadModel
                {
                    Id = chapter.Id,
                    UploadOrder = chapter.UploadOrder,
                    PublishedAt = chapter.PublishedAt
                })
                .FirstOrDefaultAsync();

            item.LatestChapter = lastestChapter;
        }

        return followedArtworks;
    }

    public async Task GetComicPaginationDetailAsync(
        long UserId,
        int PageNumber,
        int PageSize,
        List<Artwork> artworks
    )
    {
        var result = await _artworkDbSet
            .Where(a =>
                a.ArtworkType == ArtworkType.Comic
                && a.CreatedBy == UserId
                && a.IsTemporarilyRemoved == false
            )
            .Select(a => new Artwork()
            {
                Id = a.Id,
                Title = a.Title,
                ThumbnailUrl = a.ThumbnailUrl,
                UpdatedAt = a.UpdatedAt,
                AuthorName = a.AuthorName,
                Origin = new ArtworkOrigin { ImageUrl = a.Origin.ImageUrl },
                ArtworkMetaData = new ArtworkMetaData
                {
                    TotalFavorites = a.ArtworkMetaData.TotalFavorites,
                    AverageStarRate = a.ArtworkMetaData.AverageStarRate,
                },
            })
            .OrderByDescending(a => a.UpdatedAt)
            .Skip((PageNumber - 1) * PageSize)
            .Take(PageSize)
            .AsNoTracking()
            .ToListAsync();

        artworks.AddRange(result);
    }

    public async Task GetAnimePaginationDetailAsync(
        long UserId,
        int PageNumber,
        int PageSize,
        List<Artwork> artworks
    )
    {
        var result = await _artworkDbSet
            .Where(a =>
                a.ArtworkType == ArtworkType.Animation
                && a.CreatedBy == UserId
                && a.IsTemporarilyRemoved == false
            )
            .Select(a => new Artwork()
            {
                Id = a.Id,
                Title = a.Title,
                ThumbnailUrl = a.ThumbnailUrl,
                UpdatedAt = a.UpdatedAt,
                AuthorName = a.AuthorName,
                Origin = new ArtworkOrigin { ImageUrl = a.Origin.ImageUrl },
                ArtworkMetaData = new ArtworkMetaData
                {
                    TotalFavorites = a.ArtworkMetaData.TotalFavorites,
                    AverageStarRate = a.ArtworkMetaData.AverageStarRate,
                },
            })
            .OrderByDescending(a => a.UpdatedAt)
            .Skip((PageNumber - 1) * PageSize)
            .Take(PageSize)
            .AsNoTracking()
            .ToListAsync();

        artworks.AddRange(result);
    }

    public async Task GetSeriesPaginationDetailAsync(
        long UserId,
        int PageNumber,
        int PageSize,
        List<Series> series
    )
    {
        var result = await _seriesDbSet
            .Where(a => a.CreatedBy == UserId && a.IsTemporarilyRemoved == false)
            .Select(a => new Series()
            {
                Id = a.Id,
                Title = a.Title,
                ThumbnailUrl = a.ThumbnailUrl,
                UpdatedAt = a.UpdatedAt,
                Creator = new UserProfile { NickName = a.Creator.NickName },
            })
            .OrderByDescending(a => a.UpdatedAt)
            .Skip((PageNumber - 1) * PageSize)
            .Take(PageSize)
            .AsNoTracking()
            .ToListAsync();

        series.AddRange(result);
    }
}
