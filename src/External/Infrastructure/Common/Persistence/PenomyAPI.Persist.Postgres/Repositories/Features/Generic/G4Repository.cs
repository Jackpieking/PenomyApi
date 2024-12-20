using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG4;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Common.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class G4Repository : IG4Repository
{
    /// <summary>
    ///     This recommendation use naive approach, so that by using 
    ///     artworks that already viewed by the user for filtering their related categories,
    ///     this system can recommend the suitable artworks for the clients.
    /// </summary>
    private const int NUMBER_OF_TOP_ARTWORKS_TO_TAKE_FOR_FILTERING = 4;
    /// <summary>
    ///     The number of recommend categories to return.
    /// </summary>
    private const int NUMBER_OF_RECOMMENDED_CATEGORIES = 4;
    private const int NUMBER_OF_RECOMMENED_ARTWORK_PER_CATEGORY = 12;
    private const int NUMBER_OF_RECOMMENDED_NEW_CHAPTERS_PER_ARTWORK = 2;

    private readonly DbContext _dbContext;
    private readonly DbSet<Artwork> _artworkDbSet;
    private readonly DbSet<ArtworkCategory> _artworkCategoryDbSet;
    private readonly DbSet<Category> _categoryDbSet;

    #region Compiled & Raw Queries section
    /// <summary>
    ///     Return the raw SQL query to fetch the 2 latest chapters
    ///     of the specified artwork by the input <paramref name="artworkId"/>
    /// </summary>
    /// <param name="artworkId">
    ///     Id of the artwork to fetch.
    /// </param>
    /// <param name="uploadOrder">
    ///     The upload order for comparision to get the latest chapter.
    /// </param>
    /// <returns>
    ///     The raw SQL query string.
    /// </returns>
    private static FormattableString GetLatestChaptersRawQuery(long artworkId, int uploadOrder)
    {
        FormattableString rawQuery = $@"
            SELECT t.""Id"",
                t.""UploadOrder"",
                t.""PublishedAt""
            FROM penomy_artwork_chapter AS t
            WHERE t.""ArtworkId"" = {artworkId}
                AND (t.""UploadOrder"" = {uploadOrder} OR t.""UploadOrder"" <= {uploadOrder})
                AND t.""PublicLevel"" = {ArtworkPublicLevel.Everyone}
                AND t.""PublishStatus"" = {PublishStatus.Published}
            ORDER BY t.""UploadOrder"" DESC
            LIMIT {NUMBER_OF_RECOMMENDED_NEW_CHAPTERS_PER_ARTWORK}";

        return rawQuery;
    }

    private static FormattableString GetTopRecommendedArtworkIdsRawQuery(ArtworkType artworkType)
    {
        FormattableString rawQuery = $@"
            SELECT artwork.""Id""
            FROM penomy_artwork AS artwork
            INNER JOIN penomy_artwork_metadata AS metadata 
                ON artwork.""Id"" = metadata.""ArtworkId""
            WHERE 
                artwork.""ArtworkType"" = {(int) artworkType}
                AND artwork.""PublicLevel"" = {ArtworkPublicLevel.Everyone}
                AND NOT (artwork.""IsTakenDown"")
                AND NOT (artwork.""IsTemporarilyRemoved"")
            ORDER BY metadata.""TotalFollowers"" DESC
            LIMIT {NUMBER_OF_TOP_ARTWORKS_TO_TAKE_FOR_FILTERING}";

        return rawQuery;
    }

    private static string GetRecommendedCategoriesByArtworkIdsRawQuery(
        IEnumerable<long> artworkIds,
        int numberOfCategories)
    {
        string rawQuery = $@"
            SELECT 
                DISTINCT p.""CategoryId"" AS ""Id"",
                p0.""Name""
            FROM penomy_artwork_category AS p
            INNER JOIN penomy_category AS p0 ON p.""CategoryId"" = p0.""Id""
            WHERE p.""ArtworkId"" IN ({artworkIds.ToSqlArray()})
            ORDER BY p.""CategoryId""
            LIMIT {numberOfCategories}";

        return rawQuery;
    }

    private static FormattableString GetRecommendedArtworksByCategoryIdRawQuery(
        long categoryId,
        ArtworkType artworkType)
    {
        // The below comment is the original version EF Query, un-comment this when 
        // you want to debug or explore the operation, otherwise keep the raw query version.

        // var recommendedArtworks = await _artworkCategoryDbSet
        //    .Where(comicCategory
        //        => comicCategory.CategoryId == category.Id
        //        && comicCategory.Artwork.LastChapterUploadOrder > 0
        //        && !comicCategory.Artwork.IsTakenDown
        //        && !comicCategory.Artwork.IsTemporarilyRemoved
        //        && comicCategory.Artwork.PublicLevel == ArtworkPublicLevel.Everyone)
        //    .OrderByDescending(comicCategory => comicCategory.Artwork.ArtworkMetaData.TotalFollowers)
        //    .Take(NUMBER_OF_RECOMMENED_COMIC_PER_CATEGORY)
        //    .Select(comicCategory => new RecommendedComicReadModel
        //    {
        //        Id = comicCategory.ArtworkId,
        //        Title = comicCategory.Artwork.Title,
        //        ThumbnailUrl = comicCategory.Artwork.ThumbnailUrl,
        //        ArtworkStatus = comicCategory.Artwork.ArtworkStatus,
        //        LastChapterUploadOrder = comicCategory.Artwork.LastChapterUploadOrder,
        //        OriginImageUrl = comicCategory.Artwork.Origin.ImageUrl,
        //        TotalStarRates = comicCategory.Artwork.ArtworkMetaData.TotalStarRates,
        //        TotalUsersRated = comicCategory.Artwork.ArtworkMetaData.TotalUsersRated,
        //        CreatorId = comicCategory.Artwork.Creator.UserId,
        //        CreatorName = comicCategory.Artwork.Creator.NickName,
        //        CreatorAvatarUrl = comicCategory.Artwork.Creator.AvatarUrl,
        //    })
        //    .ToListAsync(cancellationToken);

        FormattableString rawQuery = $@"
            SELECT 
                filtered_artwork.""ArtworkId"" AS ""Id"",
                filtered_artwork.""Title"",
                filtered_artwork.""ThumbnailUrl"",
                filtered_artwork.""ArtworkStatus"",
                filtered_artwork.""LastChapterUploadOrder"",
                origin.""ImageUrl"" AS ""OriginImageUrl"",
                filtered_artwork.""TotalStarRates"",
                filtered_artwork.""TotalUsersRated"",
                creator.""UserId"" AS ""CreatorId"",
                creator.""NickName"" AS ""CreatorName"",
                creator.""AvatarUrl"" AS ""CreatorAvatarUrl""
            FROM
                (SELECT 
                    p.""ArtworkId"",
                    p0.""ArtworkOriginId"",
                    p0.""ArtworkStatus"",
                    p0.""CreatedBy"",
                    p0.""LastChapterUploadOrder"",
                    p0.""ThumbnailUrl"",
                    p0.""Title"",
                    p1.""TotalFollowers"",
                    p1.""TotalStarRates"",
                    p1.""TotalUsersRated""
                FROM penomy_artwork_category AS p
                INNER JOIN penomy_artwork AS p0 ON p.""ArtworkId"" = p0.""Id""
                INNER JOIN penomy_artwork_metadata AS p1 ON p0.""Id"" = p1.""ArtworkId""
                WHERE p.""CategoryId"" = {categoryId}
                     AND p0.""ArtworkType"" = {(int) artworkType}
                     AND p0.""LastChapterUploadOrder"" > 0
                     AND NOT (p0.""IsTakenDown"")
                     AND NOT (p0.""IsTemporarilyRemoved"")
                     AND p0.""PublicLevel"" = {ArtworkPublicLevel.Everyone}
                ORDER BY p1.""TotalFollowers"" DESC
                LIMIT {NUMBER_OF_RECOMMENED_ARTWORK_PER_CATEGORY}) AS filtered_artwork
            INNER JOIN penomy_artwork_origin AS origin ON filtered_artwork.""ArtworkOriginId"" = origin.""Id""
            INNER JOIN penomy_user_profile AS creator ON filtered_artwork.""CreatedBy"" = creator.""UserId"";";

        return rawQuery;

    }

    private static string GetMoreRecommendedCategoriesRawQuery(
        IEnumerable<long> existedCategoryIds,
        int numberToTakeMore)
    {
        // The below comment is the original version EF Query, un-comment this when 
        // you want to debug or explore the operation, otherwise keep the raw query version.

        //  var categoryToTakeMore = await _dbContext.Set<ArtworkCategory>()
        //    .Where(artworkCategory =>
        //        !recommendedCategoryIds.Contains(artworkCategory.CategoryId)
        //        && artworkCategory.Artwork.PublicLevel == ArtworkPublicLevel.Everyone
        //        && !artworkCategory.Artwork.IsTakenDown
        //        && !artworkCategory.Artwork.IsTemporarilyRemoved)
        //    .Select(artworkCategory => new RecommendedCategoryReadModel
        //    {
        //        Id = artworkCategory.CategoryId,
        //        Name = artworkCategory.Category.Name,
        //    })
        //    .Take(numberToTakeMore)
        //    .ToListAsync(cancellationToken);

        var sqlArray = existedCategoryIds.ToSqlArray();

        string rawQuery = $@"
            SELECT t.""CategoryId"" AS ""Id"", p1.""Name""
            FROM (
                SELECT p.""CategoryId""
                FROM penomy_artwork_category AS p
                INNER JOIN penomy_artwork AS p0 ON p.""ArtworkId"" = p0.""Id""
                WHERE
                    NOT (p.""CategoryId"" IN ({sqlArray}))
                    AND p0.""PublicLevel"" = {(int) ArtworkPublicLevel.Everyone}
                    AND NOT (p0.""IsTakenDown"")
                    AND NOT (p0.""IsTemporarilyRemoved"")
                LIMIT {numberToTakeMore}
            ) AS t
            INNER JOIN penomy_category AS p1 ON t.""CategoryId"" = p1.""Id""";

        return rawQuery;
    }
    #endregion

    public G4Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _artworkDbSet = dbContext.Set<Artwork>();
        _artworkCategoryDbSet = dbContext.Set<ArtworkCategory>();
        _categoryDbSet = dbContext.Set<Category>();
    }

    public Task<bool> IsCurrentGuestHasViewHistoryAsync(long guestId, CancellationToken cancellationToken)
    {
        var guestViewHistoryDbSet = _dbContext.Set<GuestArtworkViewHistory>();

        return guestViewHistoryDbSet.AnyAsync(
            viewHistory => viewHistory.GuestId == guestId,
            cancellationToken);
    }

    #region For New Guest
    public async Task<List<RecommendedArtworkByCategory>> GetRecommendedArtworksForNewGuestAsync(
        ArtworkType artworkType,
        CancellationToken cancellationToken)
    {
        // To recommend for the new guest, the platform will base on the top 4 recommended artworks.
        // Take the categories of these artwork and return the artworks that fall in these categories.
        List<long> topRecommendedArtworkIds = await InternalGetTopRecommendedArtworkIdsAsync(
            artworkType,
            cancellationToken);

        // Get significant categories in the view history artwork list
        // then based on that to recommend the artworks to the guest.
        List<RecommendedCategoryReadModel> recommendedCategories =
            await InternalGetRecommendedCategoriesAsync(
                topRecommendedArtworkIds,
                NUMBER_OF_RECOMMENDED_CATEGORIES,
                cancellationToken);

        var resultList = await InternalGetRecommendedArtworksByCategoriesAsync(
            recommendedCategories,
            artworkType,
            cancellationToken);

        return resultList;
    }
    #endregion

    #region For Signed-In User
    public async Task<List<RecommendedArtworkByCategory>> GetRecommendedArtworksForUserAsync(
        long userId,
        ArtworkType artworkType,
        CancellationToken cancellationToken)
    {
        var userViewHistoryDbSet = _dbContext.Set<UserArtworkViewHistory>();

        // Check if the current user has viewed any artwork or not to recommend properly.
        var totalHistoryItems = await userViewHistoryDbSet.CountAsync(
            viewHistory => viewHistory.UserId == userId,
            cancellationToken);

        var userHasViewHistory = totalHistoryItems >= 2;

        // If current user has not viewed any artwork yet, then recommend using default algoritm.
        if (!userHasViewHistory)
        {
            return await GetRecommendedArtworksForNewGuestAsync(artworkType, cancellationToken);
        }

        // Get the artworkId list from the view history, based from that to recommend for this user.
        List<long> artworkIds = await userViewHistoryDbSet
            .AsNoTracking()
            .Where(viewHistory
                => viewHistory.UserId == userId)
            .Select(viewHistory => viewHistory.ArtworkId)
            .Take(NUMBER_OF_TOP_ARTWORKS_TO_TAKE_FOR_FILTERING)
            .ToListAsync(cancellationToken);

        // Get significant categories in the view history artwork list
        // then based on that to recommend the artworks to the guest.
        List<RecommendedCategoryReadModel> recommendedCategories =
            await InternalGetRecommendedCategoriesAsync(
                artworkIds,
                NUMBER_OF_RECOMMENDED_CATEGORIES,
                cancellationToken);

        // If the recommended category list length is less than the NUMBER_OF_RECOMMENDED_CATEGORIES
        // then must take enough number of category to recommended for user.
        var lessThanMinimumRecommendedNumber = recommendedCategories.Count < NUMBER_OF_RECOMMENDED_CATEGORIES;

        if (lessThanMinimumRecommendedNumber)
        {
            int numberToTakeMore = NUMBER_OF_RECOMMENDED_CATEGORIES - recommendedCategories.Count;
            var currentCategoryIds = recommendedCategories.Select(c => c.Id);

            var rawSqlQuery = GetMoreRecommendedCategoriesRawQuery(
                currentCategoryIds,
                numberToTakeMore);

            var categoryToTakeMore = await _dbContext.Database
                .SqlQueryRaw<RecommendedCategoryReadModel>(rawSqlQuery)
                .ToListAsync(cancellationToken);

            recommendedCategories.AddRange(categoryToTakeMore);
        }

        var resultList = await InternalGetRecommendedArtworksByCategoriesAsync(
            recommendedCategories,
            artworkType,
            cancellationToken);

        return resultList;
    }
    #endregion

    #region For Already-Visited Guest
    public async Task<List<RecommendedArtworkByCategory>> GetRecommendedArtworksForGuestAsync(
        long guestId,
        ArtworkType artworkType,
        CancellationToken cancellationToken)
    {
        var guestViewHistoryDbSet = _dbContext.Set<GuestArtworkViewHistory>();

        // Check if the current guest has viewed any artwork or not to recommend properly.
        var userHasViewHistory = await guestViewHistoryDbSet.AnyAsync(
            viewHistory => viewHistory.GuestId == guestId,
            cancellationToken);

        // If current guest has not viewed any artwork yet, then recommend using default algoritm.
        if (!userHasViewHistory)
        {
            return await GetRecommendedArtworksForNewGuestAsync(artworkType, cancellationToken);
        }

        // Get the artworkId list from the view history, based from that to recommend for this user.
        List<long> artworkIds = await guestViewHistoryDbSet
        .AsNoTracking()
            .Where(viewHistory
                => viewHistory.GuestId == guestId
                && viewHistory.ArtworkType == artworkType)
            .Select(viewHistory => viewHistory.ArtworkId)
            .Take(NUMBER_OF_TOP_ARTWORKS_TO_TAKE_FOR_FILTERING)
            .ToListAsync(cancellationToken);

        // Get significant categories in the view history artwork list
        // then based on that to recommend the artworks to the guest.
        List<RecommendedCategoryReadModel> recommendedCategories =
            await InternalGetRecommendedCategoriesAsync(
                artworkIds,
                NUMBER_OF_RECOMMENDED_CATEGORIES,
                cancellationToken);

        // If the recommended category list length is less than the NUMBER_OF_RECOMMENDED_CATEGORIES
        // then must take enough number of category to recommended for user.
        var lessThanMinimumRecommendedNumber = recommendedCategories.Count < NUMBER_OF_RECOMMENDED_CATEGORIES;

        if (lessThanMinimumRecommendedNumber)
        {
            int numberToTakeMore = NUMBER_OF_RECOMMENDED_CATEGORIES - recommendedCategories.Count;
            var currentCategoryIds = recommendedCategories.Select(c => c.Id);

            var rawSqlQuery = GetMoreRecommendedCategoriesRawQuery(
                currentCategoryIds,
                numberToTakeMore);

            var categoryToTakeMore = await _dbContext.Database
                .SqlQueryRaw<RecommendedCategoryReadModel>(rawSqlQuery)
                .ToListAsync(cancellationToken);

            recommendedCategories.AddRange(categoryToTakeMore);
        }

        var resultList = await InternalGetRecommendedArtworksByCategoriesAsync(
            recommendedCategories,
            artworkType,
            cancellationToken);

        return resultList;
    }
    #endregion

    #region Private Methods
    /// <summary>
    ///     Get the list of categories that will be used for
    ///     later operation with recommendation algorithm.
    /// </summary>
    /// <param name="referencedArtworkIds">
    ///     The list of referenced artwork ids to take the category from.
    /// </param>
    /// <param name="numberOfCategories">
    ///     The number of categories to take for recommendation.
    /// </param>
    private Task<List<RecommendedCategoryReadModel>> InternalGetRecommendedCategoriesAsync(
        IEnumerable<long> referencedArtworkIds,
        int numberOfCategories,
        CancellationToken cancellationToken)
    {
        var rawQuery = GetRecommendedCategoriesByArtworkIdsRawQuery(
            referencedArtworkIds,
            numberOfCategories);

        return _dbContext.Database
            .SqlQueryRaw<RecommendedCategoryReadModel>(rawQuery)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    private async Task<List<RecommendedArtworkByCategory>> InternalGetRecommendedArtworksByCategoriesAsync(
        List<RecommendedCategoryReadModel> recommendedCategories,
        ArtworkType artworkType,
        CancellationToken cancellationToken)
    {
        var resultList = new List<RecommendedArtworkByCategory>();
        // This recommended comic list will be used for later retrieving its new chapters.
        var recommendedComicList = new List<RecommendedArtworkReadModel>();

        // Get the recommended comics that belonged to each category.
        foreach (var category in recommendedCategories)
        {
            var rawQuery = GetRecommendedArtworksByCategoryIdRawQuery(category.Id, artworkType);

            var recommendedArtworks = await _dbContext.Database
                .SqlQuery<RecommendedArtworkReadModel>(rawQuery)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            if (recommendedArtworks.Any())
            {
                resultList.Add(new RecommendedArtworkByCategory
                {
                    Category = category,
                    RecommendedArtworks = recommendedArtworks
                });

                recommendedComicList.AddRange(recommendedArtworks);
            }
        }

        // Load the new chapters for each recommended comics.
        foreach (var currentComic in recommendedComicList)
        {
            var newChapterList = await InternalGetNewChapterListAsync(
                currentComic,
                cancellationToken);

            // Set the new chapter list for this comic detail.
            currentComic.NewChapters = newChapterList;
        }

        return resultList;
    }

    /// <summary>
    ///     Get the list of latest chapter of the specified
    ///     comic by the provided <paramref name="comicDetail"/>.
    /// </summary>
    /// <param name="comicDetail">
    ///     The artwork that needed to get the latest chapter list.
    /// </param>
    private Task<List<G4NewChapterReadModel>> InternalGetNewChapterListAsync(
        RecommendedArtworkReadModel comicDetail,
        CancellationToken cancellationToken)
    {
        var rawQuery = GetLatestChaptersRawQuery(
            comicDetail.Id,
            comicDetail.LastChapterUploadOrder);

        return _dbContext.Database
            .SqlQuery<G4NewChapterReadModel>(rawQuery)
            .ToListAsync(cancellationToken);
    }

    private Task<List<long>> InternalGetTopRecommendedArtworkIdsAsync(
        ArtworkType artworkType,
        CancellationToken cancellationToken)
    {
        var rawQuery = GetTopRecommendedArtworkIdsRawQuery(artworkType);

        return _dbContext.Database
            .SqlQuery<long>(rawQuery)
            .ToListAsync(cancellationToken);
    }
    #endregion
}
