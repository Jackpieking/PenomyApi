using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Models.ArtworkCreation.FeatArt1;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.ArtworkCreation;

internal sealed class Art1Repository : IArt1Repository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<Artwork> _artworkDbSet;

    public Art1Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _artworkDbSet = dbContext.Set<Artwork>();
    }

    public Task<List<Artwork>> GetArtworksByTypeAndCreatorIdWithPaginationAsync(
        ArtworkType artworkType,
        long creatorId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        // The page number will reduce 1 to applie the zero-based count.
        return _artworkDbSet
            .AsNoTracking()
            .Where(artwork =>
                !artwork.IsTemporarilyRemoved
                && artwork.ArtworkType == artworkType
                && artwork.CreatedBy == creatorId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .OrderBy(artwork => artwork.CreatedAt)
            .Select(artwork => new Artwork
            {
                Id = artwork.Id,
                Title = artwork.Title,
                ArtworkStatus = artwork.ArtworkStatus,
                PublicLevel = artwork.PublicLevel,
                ThumbnailUrl = artwork.ThumbnailUrl,
                LastChapterUploadOrder = artwork.LastChapterUploadOrder,
                ArtworkMetaData = new ArtworkMetaData
                {
                    AverageStarRate = artwork.ArtworkMetaData.AverageStarRate,
                    TotalComments = artwork.ArtworkMetaData.TotalComments,
                    TotalFavorites = artwork.ArtworkMetaData.TotalFavorites,
                    TotalFollowers = artwork.ArtworkMetaData.TotalFollowers,
                    TotalUsersRated = artwork.ArtworkMetaData.TotalUsersRated,
                    TotalViews = artwork.ArtworkMetaData.TotalViews,
                },
                CreatedAt = artwork.CreatedAt,
                UpdatedAt = artwork.UpdatedAt,
            })
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public Task<int> GetTotalOfArtworksByTypeAndCreatorIdAsync(
        ArtworkType artworkType,
        long creatorId,
        CancellationToken cancellationToken)
    {
        return _artworkDbSet.CountAsync(
            predicate: artwork =>
                artwork.ArtworkType == artworkType
                && artwork.CreatedBy == creatorId,
            cancellationToken: cancellationToken);
    }

    public async Task<OverviewStatisticReadModel> GetOverviewStatisticByCreatorIdAsync(
        long creatorId,
        CancellationToken cancellationToken)
    {
        var totalComics = await _artworkDbSet
            .CountAsync(artwork =>
                artwork.CreatedBy == creatorId
                && artwork.ArtworkType == ArtworkType.Comic
                && !artwork.IsTemporarilyRemoved);

        var totalAnimations = await _artworkDbSet
            .CountAsync(artwork =>
                artwork.CreatedBy == creatorId
                && artwork.ArtworkType == ArtworkType.Animation
                && !artwork.IsTemporarilyRemoved);

        var totalSeries = await _dbContext.Set<Series>()
            .CountAsync(series =>
                series.CreatedBy == creatorId
                && !series.IsTemporarilyRemoved);

        return new OverviewStatisticReadModel
        {
            TotalComics = totalComics,
            TotalAnimations = totalAnimations,
            TotalSeries = totalSeries
        };
    }
}
