using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class G28Repository : IG28Repository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<Artwork> _artworkDbSet;

    public G28Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _artworkDbSet = dbContext.Set<Artwork>();
    }

    public async Task<long> GetPaginationOptionsByArtworkTypeAsync(long artworkType, long UserId)
    {
        if (artworkType == 1)
        {
            var count = await _artworkDbSet
                .Where(a => a.ArtworkType == ArtworkType.Comic && a.CreatedBy == UserId)
                .CountAsync();
            return count / 8;
        }
        else if (artworkType == 2)
        {
            var count = await _artworkDbSet
                .Where(a => a.ArtworkType == ArtworkType.Animation && a.CreatedBy == UserId)
                .CountAsync();
            return count / 8;
        }
        else
            return 0;
    }

    public async Task<List<Artwork>> GetPaginationDetailAsync(
        long UserId,
        long ArtworkType,
        int PageNumber,
        int PageSize
    )
    {
        List<Artwork> artworks = new List<Artwork>();
        if (ArtworkType == 1)
            await GetComicPaginationDetailAsync(UserId, PageNumber, PageSize, artworks);
        else if (ArtworkType == 2)
            await GetAnimePaginationDetailAsync(UserId, PageNumber, PageSize, artworks);
        return artworks;
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
}
