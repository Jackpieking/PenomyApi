using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

internal sealed class G25Repository : IG25Repository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<UserArtworkViewHistory> _viewHistDbSet;
    private readonly DbSet<Artwork> _artworkDbSet;

    public G25Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _viewHistDbSet = dbContext.Set<UserArtworkViewHistory>();
        _artworkDbSet = dbContext.Set<Artwork>();
    }

    public async Task<int> ArtworkHistoriesCount(
        long userId,
        ArtworkType artType,
        CancellationToken ct
    )
    {
        return await _viewHistDbSet
            .AsNoTracking()
            .Where(viewHist => viewHist.UserId == userId && viewHist.ArtworkType == artType)
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
        return await _viewHistDbSet
            .AsNoTracking()
            .Where(viewHist => viewHist.UserId == userId && viewHist.ArtworkType == artType)
            .OrderByDescending(viewHist => viewHist.ViewedAt) // Order by last view chapter
            .GroupBy(viewHist => viewHist.ArtworkId)
            .Skip((pageNum - 1) * artNum)
            .Take(artNum)
            .Select(grp =>
                grp.Select(viewHist => new UserArtworkViewHistory
                {
                    ArtworkId = viewHist.ArtworkId,
                    ArtworkType = viewHist.ArtworkType,
                    Artwork = new Artwork
                    {
                        Title = viewHist.Artwork.Title,
                        CreatedBy = viewHist.Artwork.CreatedBy,
                        AuthorName = viewHist.Artwork.AuthorName,
                        ThumbnailUrl = viewHist.Artwork.ThumbnailUrl,
                        ArtworkMetaData = new ArtworkMetaData
                        {
                            TotalFavorites = viewHist.Artwork.ArtworkMetaData.TotalFavorites,
                            TotalStarRates = viewHist.Artwork.ArtworkMetaData.TotalStarRates
                        },
                    },
                    Chapter = new ArtworkChapter
                    {
                        Id = viewHist.Artwork.Id,
                        Title = viewHist.Chapter.Title,
                        UploadOrder = viewHist.Chapter.UploadOrder,
                        ThumbnailUrl = viewHist.Chapter.ThumbnailUrl
                    },
                    ViewedAt = viewHist.ViewedAt
                })
            )
            .ToListAsync(ct);
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
            var readViewHist = _viewHistDbSet.AsNoTracking();

            // If the chapter has viewed the chapter update time
            // If not add new history
            if (readViewHist.Any(o => o.UserId == userId && o.ChapterId == chapterId))
            {
                await _viewHistDbSet
                    .Where(o => o.UserId == userId && o.ChapterId == chapterId)
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

            _dbContext.SaveChanges();
        }
        catch
        {
            return false;
        }

        return true;
    }
}
