using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG3;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class FeatG3Repository : IFeatG3Repository
{
    private const int NUMBER_OF_ARTWORKS_TO_RETURN = 16;
    private const int NUMBER_OF_RECOMMENDED_CHAPTERS_PER_ARTWORK = 2;

    private readonly DbContext _dbContext;
    private readonly DbSet<Artwork> _artworkDbSet;
    private readonly DbSet<ArtworkMetaData> _statisticDbSet;

    public FeatG3Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _artworkDbSet = dbContext.Set<Artwork>();
        _statisticDbSet = dbContext.Set<ArtworkMetaData>();
    }

    #region Raw Queries section
    private static FormattableString GetRecentlyUpdatedComicsRawQuery()
    {
        FormattableString rawQuery = $@"
            SELECT 
                t.""Id"",
                t.""Title"",
                t.""ThumbnailUrl"",
                t.""ArtworkStatus"",
                t.""UpdatedAt"",
                t.""LastChapterUploadOrder"",
                p1.""ImageUrl"" AS ""OriginImageUrl"",
                p0.""UserId"",
                p0.""AvatarUrl"",
                p0.""NickName"",
                p1.""ImageUrl"",
                p2.""TotalStarRates"",
                p2.""TotalUsersRated"",
                p0.""UserId"" AS ""CreatorId"",
                p0.""NickName"" AS ""CreatorName"",
                p0.""AvatarUrl"" AS ""CreatorAvatarUrl""
            FROM penomy_artwork AS t
            INNER JOIN penomy_user_profile AS p0 ON t.""CreatedBy"" = p0.""UserId""
            INNER JOIN penomy_artwork_origin AS p1 ON t.""ArtworkOriginId"" = p1.""Id""
            INNER JOIN penomy_artwork_metadata AS p2 ON t.""Id"" = p2.""ArtworkId""
            WHERE 
                t.""ArtworkType"" = {ArtworkType.Comic}
                AND t.""PublicLevel"" = {ArtworkPublicLevel.Everyone}
                AND NOT (t.""IsTakenDown"")
                AND NOT (t.""IsTemporarilyRemoved"")
                AND t.""LastChapterUploadOrder"" > 0
            ORDER BY p2.""TotalStarRates"" DESC, t.""UpdatedAt"" DESC
            LIMIT {NUMBER_OF_ARTWORKS_TO_RETURN}";

        return rawQuery;
    }

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
            LIMIT {NUMBER_OF_RECOMMENDED_CHAPTERS_PER_ARTWORK}";

        return rawQuery;
    }
    #endregion

    public async Task<List<RecentlyUpdatedComicReadModel>> GetRecentlyUpdatedComicsAsync(
        CancellationToken cancellationToken)
    {
        var rawQuery = GetRecentlyUpdatedComicsRawQuery();

        var resultList = await _dbContext.Database
            .SqlQuery<RecentlyUpdatedComicReadModel>(rawQuery)
            .ToListAsync();

        // Load the new chapters for each recommended comics.
        foreach (var currentComic in resultList)
        {
            var newChapterList = await InternalGetNewChapterListAsync(
                currentComic,
                cancellationToken);

            // Set the new chapter list for this comic detail.
            currentComic.NewChapters = newChapterList;
        }

        return resultList;
    }

    #region Private Methods
    /// <summary>
    ///     Get the list of latest chapter of the specified
    ///     comic by the provided <paramref name="comicDetail"/>.
    /// </summary>
    /// <param name="comicDetail">
    ///     The artwork that needed to get the latest chapter list.
    /// </param>
    private Task<List<G3NewChapterReadModel>> InternalGetNewChapterListAsync(
        RecentlyUpdatedComicReadModel comicDetail,
        CancellationToken cancellationToken)
    {
        var rawQuery = GetLatestChaptersRawQuery(
            comicDetail.Id,
            comicDetail.LastChapterUploadOrder);

        return _dbContext.Database
            .SqlQuery<G3NewChapterReadModel>(rawQuery)
            .ToListAsync(cancellationToken);
    }
    #endregion
}
