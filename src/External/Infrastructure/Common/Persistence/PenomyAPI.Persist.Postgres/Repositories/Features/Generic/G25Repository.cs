using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

internal sealed class G25Repository : IG25Repository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<UserArtworkViewHistory> _viewHistDbSet;
    private readonly DbSet<ArtworkMetaData> _artworkMetaDataDbSet;

    public G25Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _viewHistDbSet = dbContext.Set<UserArtworkViewHistory>();
        _artworkMetaDataDbSet = dbContext.Set<ArtworkMetaData>();
    }

    public async Task<int> ArtworkHistoriesCount(
        long userId,
        ArtworkType artType,
        CancellationToken ct
    )
    {
        return await _viewHistDbSet
            .AsNoTracking()
            .Where(viewHist => viewHist.UserId == userId && viewHist.ArtworkType == artType && !viewHist.Artwork.IsTakenDown && !viewHist.Artwork.IsTemporarilyRemoved)
            .GroupBy(viewHist => viewHist.ArtworkId)
            .CountAsync(ct);
    }

    public async Task<IEnumerable<IEnumerable<UserArtworkViewHistory>>> GetArtworkViewHistories(
        long userId,
        ArtworkType artType,
        CancellationToken ct,
        int pageNum = 1,
        int artNum = 20
    )
    {
        var viewHist = await _viewHistDbSet
           .AsNoTracking()
           .Where(viewHist => viewHist.UserId == userId && viewHist.ArtworkType == artType && !viewHist.Artwork.IsTakenDown && !viewHist.Artwork.IsTemporarilyRemoved)
           .OrderByDescending(viewHist => viewHist.ViewedAt) // Order by last view chapter
           .GroupBy(viewHist => viewHist.ArtworkId)
           .Skip((pageNum - 1) * artNum)
           .Take(artNum)
           .Select(grp =>
               grp.Select(viewHist => new UserArtworkViewHistory
               {
                   ArtworkId = viewHist.ArtworkId,
                   Artwork = new Artwork
                   {
                       Title = viewHist.Artwork.Title,
                       CreatedBy = viewHist.Artwork.CreatedBy,
                       AuthorName = viewHist.Artwork.AuthorName,
                       ThumbnailUrl = viewHist.Artwork.ThumbnailUrl,
                       ArtworkMetaData = new ArtworkMetaData
                       {
                           TotalFavorites = viewHist.Artwork.ArtworkMetaData.TotalFavorites,
                           AverageStarRate = viewHist.Artwork.ArtworkMetaData.GetAverageStarRate()
                       },
                       Origin = new ArtworkOrigin
                       {
                           ImageUrl = viewHist.Artwork.Origin.ImageUrl,
                       }
                   },
                   Chapter = new ArtworkChapter
                   {
                       Id = viewHist.Chapter.Id,
                       Title = viewHist.Chapter.Title,
                       UploadOrder = viewHist.Chapter.UploadOrder,
                   },
                   ViewedAt = viewHist.ViewedAt
               })
           )
           .ToListAsync(ct);

        return viewHist;
    }

    public async Task<bool> AddArtworkViewHist(
        long userId,
        long artworkId,
        long chapterId,
        ArtworkType type,
        CancellationToken ct,
        int limitChapter = 5
    )
    {
        try
        {
            var readViewHist = _viewHistDbSet;

            // If the chapter has viewed update the chapter view time
            // If not add new history
            if (readViewHist.AsNoTracking().Any(o => o.UserId == userId && o.ArtworkId == artworkId && o.ChapterId == chapterId))
            {
                await _viewHistDbSet
                    .Where(o => o.UserId == userId && o.ArtworkId == artworkId && o.ChapterId == chapterId)
                    .ExecuteUpdateAsync(setters =>
                        setters.SetProperty(o => o.ViewedAt, DateTime.UtcNow)
                    );
            }
            else
            {
                await _viewHistDbSet.AddAsync(
                    new UserArtworkViewHistory
                    {
                        UserId = userId,
                        ArtworkId = artworkId,
                        ChapterId = chapterId,
                        ArtworkType = type,
                        ViewedAt = DateTime.UtcNow
                    },
                    ct
                );

                // Limit the history for one art to n chapters
                if (readViewHist.Where(o => o.ArtworkId == artworkId).Count() > limitChapter)
                {
                    await _viewHistDbSet
                        .Where(o => o.ArtworkId == artworkId)
                        .OrderByDescending(o => o.ViewedAt)
                        .Skip(limitChapter)
                        .ExecuteDeleteAsync();
                }
            }

            await _artworkMetaDataDbSet
                .Where(o => o.ArtworkId == artworkId)
                .ExecuteUpdateAsync(o => o.SetProperty(o => o.TotalViews, e => e.TotalViews + 1));

            _dbContext.SaveChanges();
        }
        catch
        {
            return false;
        }

        return true;
    }
}
