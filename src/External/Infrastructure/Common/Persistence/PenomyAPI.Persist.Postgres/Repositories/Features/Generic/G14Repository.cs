using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Data.DbContexts;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class G14Repository : IG14Repository
{
    private readonly AppDbContext _dbContext;
    private readonly DbSet<Artwork> _artworkSet;
    private readonly DbSet<Category> _categorySet;
    private readonly DbSet<UserFavoriteArtwork> _userFavoriteSet;
    private readonly DbSet<UserFollowedArtwork> _userFollowSet;
    private readonly DbSet<UserArtworkViewHistory> _userHistorySet;
    private readonly DbSet<GuestArtworkViewHistory> _guestHistorySet;
    public G14Repository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _artworkSet = _dbContext.Set<Artwork>();
        _categorySet = _dbContext.Set<Category>();
        _userFavoriteSet = _dbContext.Set<UserFavoriteArtwork>();
        _userFollowSet = _dbContext.Set<UserFollowedArtwork>();
        _userHistorySet = _dbContext.Set<UserArtworkViewHistory>();
        _guestHistorySet = _dbContext.Set<GuestArtworkViewHistory>();
    }

    public async Task<List<Artwork>> GetRecommendedAnimeAsync(long cateId, CancellationToken cancellationToken = default)
    {
        var result = await _artworkSet
            .Where(x => x.ArtworkType == ArtworkType.Comic && x.ArtworkCategories.Any(y => y.CategoryId == cateId))
            .Take(18)
            .Select(x => new Artwork
            {
                Title = x.Title,
                AuthorName = x.AuthorName,
                Introduction = x.Introduction,
                Id = x.Id,
                Origin = new ArtworkOrigin
                {
                    Id = x.Origin.Id,
                    CountryName = x.Origin.CountryName,
                },
                ArtworkCategories = x.ArtworkCategories.Select(y => new ArtworkCategory
                {
                    Category = new Category { Name = y.Category.Name },
                    ArtworkId = y.ArtworkId,
                    CategoryId = y.CategoryId,
                }).ToList(),
                ArtworkSeries = x.ArtworkSeries.Select(y => new ArtworkSeries
                {
                    ArtworkId = y.ArtworkId,
                    Series = y.Series,
                }).ToList(),
                ArtworkStatus = x.ArtworkStatus,
                UserRatingArtworks = x.UserRatingArtworks.Select(y => new UserRatingArtwork
                {
                    StarRates = y.StarRates,
                }).ToList(),
                ArtworkMetaData = new ArtworkMetaData
                {
                    TotalComments = x.ArtworkMetaData.TotalComments,
                    TotalFavorites = x.ArtworkMetaData.TotalFavorites,
                    TotalViews = x.ArtworkMetaData.TotalViews,
                    TotalStarRates = x.ArtworkMetaData.TotalStarRates,
                    TotalUsersRated = x.ArtworkMetaData.TotalUsersRated,
                    AverageStarRate = x.ArtworkMetaData.AverageStarRate,
                },
                ThumbnailUrl = x.ThumbnailUrl,
            })
            .OrderByDescending(x => x.ArtworkMetaData.TotalStarRates)
            .AsNoTracking()
            .AsSplitQuery()
            .ToListAsync(cancellationToken);

        return result;
    }

    public async Task<List<Category>> GetUserFavoritesCategoryIdsAsync(long userId, int totalCategoriesToTake = 4, CancellationToken token = default)
    {
        var favoriteCategoryIds = await GetCategoryIdsFromFavoritesAsync(userId, token);
        if (favoriteCategoryIds.Any())
        {
            return await GetCategoriesAsync(favoriteCategoryIds, totalCategoriesToTake, token);
        }

        var followedCategoryIds = await GetCategoryIdsFromFollowsAsync(userId, token);
        if (followedCategoryIds.Any())
        {
            return await GetCategoriesAsync(followedCategoryIds, totalCategoriesToTake, token);
        }

        var historyCategoryIds = await GetCategoryIdsFromViewHistoryAsync(userId, token);
        return await GetCategoriesAsync(historyCategoryIds, totalCategoriesToTake, token);
    }

    public async Task<bool> IsExistCategoryAsync(long cateId, CancellationToken token = default)
    {
        return await _dbContext.Set<Category>().AnyAsync(x => x.Id == cateId, token);
    }
    private async Task<List<long>> GetCategoryIdsFromFavoritesAsync(long userId, CancellationToken token)
    {
        return await _userFavoriteSet
            .Where(f => f.UserId == userId)
            .SelectMany(f => f.FavoriteArtwork.ArtworkCategories.Select(ac => ac.CategoryId))
            .ToListAsync(token);
    }
    private async Task<List<long>> GetCategoryIdsFromFollowsAsync(long userId, CancellationToken token)
    {
        return await _userFollowSet
            .Where(f => f.UserId == userId)
            .SelectMany(f => f.FollowedArtwork.ArtworkCategories.Select(ac => ac.CategoryId))
            .ToListAsync(token);
    }

    // Helper to retrieve category IDs from the user's view history.
    private async Task<List<long>> GetCategoryIdsFromViewHistoryAsync(long userId, CancellationToken token)
    {
        return await _userHistorySet
            .Where(h => h.UserId == userId)
            .SelectMany(h => h.Artwork.ArtworkCategories.Select(ac => ac.CategoryId))
            .ToListAsync(token);
    }

    private async Task<List<Category>> GetCategoriesAsync(List<long> categoryIds, int totalCategoriesToTake, CancellationToken token)
    {
        var categories = await _categorySet
            .Where(c => categoryIds.Contains(c.Id))
            .Select(c => new Category { Id = c.Id, Name = c.Name })
            .Take(totalCategoriesToTake)
            .ToListAsync(token);

        return categories;
    }
    public async Task<List<long>> GetCategoryIdsFromGuestViewHistoryAsync(long guestId, int totalToTake = 4, CancellationToken token = default)
    {
        return await _guestHistorySet
            .Where(h => h.GuestId == guestId)
            .SelectMany(h => h.Artwork.ArtworkCategories.Select(ac => ac.CategoryId))
            .ToListAsync();
    }
}
