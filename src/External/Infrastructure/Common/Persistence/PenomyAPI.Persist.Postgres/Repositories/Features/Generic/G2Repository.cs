using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG2;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

internal sealed class G2Repository : IG2Repository
{
    // Constants that supported for the operation.
    private const int MAXIMUM_NUMBER_OF_TOP_ARTWORK_IDS = 10;
    private const int NUMBER_OF_IDS_TO_BE_RETURNED = 8;

    private readonly DbContext _dbContext;
    private readonly DbSet<Artwork> _artworkDbSet;
    private readonly DbSet<ArtworkMetaData> _metadataDbSet;
    private readonly DbSet<ArtworkChapter> _chapterDbSet;

    public G2Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _artworkDbSet = dbContext.Set<Artwork>();
        _metadataDbSet = dbContext.Set<ArtworkMetaData>();
        _chapterDbSet = dbContext.Set<ArtworkChapter>();
    }

    #region Compiled & Raw Queries section
    private static FormattableString GetTopComicIdsQuery = null;
    private static FormattableString GetTopAnimeIdsQuery = null;

    private static FormattableString GetTopArtworkIdsQuery(ArtworkType artworkType)
    {
        // If artwork type is comic, then cache the rawQuery and re-used.
        lock (_lock)
        {
            if (
                Equals(GetTopComicIdsQuery, null)
                || Equals(GetTopAnimeIdsQuery, null)
            )
            {
                const int MINIMUM_LAST_CHAPTER_ORDER_TO_DISPLAY = 1;

                // Set up for comic type.
                GetTopComicIdsQuery = $@"
                    SELECT metadata_and_artwork.""Id""
                    FROM (
                          SELECT artwork.""Id"", metadata.""TotalStarRates"", metadata.""TotalUsersRated""
                          FROM penomy_artwork AS artwork
                          INNER JOIN penomy_artwork_metadata AS metadata ON artwork.""Id"" = metadata.""ArtworkId""
                          WHERE
                                artwork.""ArtworkType"" = {ArtworkType.Comic}
                                AND NOT (artwork.""IsTakenDown"")
                                AND NOT (artwork.""IsTemporarilyRemoved"")
                                AND artwork.""PublicLevel"" = {ArtworkPublicLevel.Everyone}
                                AND artwork.""LastChapterUploadOrder"" >= {MINIMUM_LAST_CHAPTER_ORDER_TO_DISPLAY}
                          ORDER BY metadata.""TotalFollowers"" DESC, metadata.""TotalFavorites"" DESC, metadata.""TotalViews"" DESC, metadata.""TotalStarRates"" DESC
                          LIMIT {NUMBER_OF_IDS_TO_BE_RETURNED}
                    ) AS metadata_and_artwork";

                // Set up for animation type.
                GetTopAnimeIdsQuery = $@"
                    SELECT metadata_and_artwork.""Id""
                    FROM (
                          SELECT artwork.""Id"", metadata.""TotalStarRates"", metadata.""TotalUsersRated""
                          FROM penomy_artwork AS artwork
                          INNER JOIN penomy_artwork_metadata AS metadata ON artwork.""Id"" = metadata.""ArtworkId""
                          WHERE
                                artwork.""ArtworkType"" = {ArtworkType.Animation}
                                AND NOT (artwork.""IsTakenDown"")
                                AND NOT (artwork.""IsTemporarilyRemoved"")
                                AND artwork.""PublicLevel"" = {ArtworkPublicLevel.Everyone}
                                AND artwork.""LastChapterUploadOrder"" >= {MINIMUM_LAST_CHAPTER_ORDER_TO_DISPLAY}
                          ORDER BY metadata.""TotalFollowers"" DESC, metadata.""TotalFavorites"" DESC, metadata.""TotalViews"" DESC, metadata.""TotalStarRates"" DESC
                          LIMIT {NUMBER_OF_IDS_TO_BE_RETURNED}
                    ) AS metadata_and_artwork";
            }
        }

        if (artworkType == ArtworkType.Comic)
        {
            return GetTopComicIdsQuery;
        }
        else
        {
            return GetTopAnimeIdsQuery;
        }
    }

    /// <summary>
    ///     Return <see langword="true"/> if the compiled queries have already setup.
    /// </summary>
    private static bool HasSetUp = false;
    private static object _lock = new object();

    // Get latest chapter compiled query section.
    private static Func<DbContext, long, int, Task<ArtworkChapter>> GetLatestArtworkChapterCompiledQuery;

    /// <summary>
    ///     Set up the compiled queries for later operation.
    /// </summary>
    private static void SetUpCompiledQueries()
    {
        if (HasSetUp)
        {
            return;
        }

        // Lock to set up safely.
        lock (_lock)
        {
            // Init GetLatestArtworkChapterExpression to create compiled query version.
            Expression<Func<DbContext, long, int, ArtworkChapter>> GetLatestArtworkChapterExpression =
                (DbContext dbContext, long artworkId, int lastChapterUploadOrder)
                    => dbContext.Set<ArtworkChapter>()
                    .AsNoTracking()
                    .Where(
                        chapter => chapter.ArtworkId == artworkId
                        // Take the latest chapter or the chapter near the latest.
                        && (chapter.UploadOrder == lastChapterUploadOrder
                            || chapter.UploadOrder <= lastChapterUploadOrder)
                        && chapter.PublicLevel == ArtworkPublicLevel.Everyone
                        && chapter.PublishStatus == PublishStatus.Published)
                    .OrderByDescending(chapter => chapter.UploadOrder)
                    .Select(chapter => new ArtworkChapter
                    {
                        Id = chapter.Id,
                        Title = chapter.Title,
                    })
                    .FirstOrDefault();

            GetLatestArtworkChapterCompiledQuery = EF.CompileAsyncQuery(
                GetLatestArtworkChapterExpression);

            // Set the flag to prevent set up multiple times.
            HasSetUp = true;

            // Clear the expression after setup success.
            GetLatestArtworkChapterExpression = null;
        }
    }
    #endregion

    public async Task<ICollection<G2TopArtworkReadModel>> GetTopRecommendedArtworksByTypeAsync(
        ArtworkType artworkType,
        CancellationToken cancellationToken)
    {
        // Set up the compilded queries before execution.
        SetUpCompiledQueries();

        // This method execution process have 3 steps:
        // + Step 1: Has 2 sub-steps:
        //  a. Filter the list of 10 candidate IDs from the artwork-metadata table
        // based on the filter criteria: TotalFollow, TotalFavorites, AverageStarRate.
        // 
        //  b. After getting the filter IDs list, only get 8 of them to display
        //  (If the return list has less than 8 items, then return all).
        //
        //  (*) All of the above filter process is conduct by the Raw SQL Query.
        //
        // + Step 2: Filter from the artwork table all artworks with the specified IDs in the
        // list of candidate IDs and get the detail of each artwork.
        // 
        // + Step 3: Get the latest chapter of each recommended artwork.

        // Execute step 1:
        var getTopArtworkIdsRawQuery = GetTopArtworkIdsQuery(artworkType);

        var candidateArtworkIds = await _dbContext.Database
            .SqlQuery<long>(getTopArtworkIdsRawQuery)
            .ToListAsync(cancellationToken);

        // Execute step 2:
        var candidateArtworkDictionary = await _artworkDbSet
            .AsNoTracking()
            .Where(artwork => candidateArtworkIds.Contains(artwork.Id))
            .Include(artwork => artwork.Origin)
            .Include(artwork => artwork.ArtworkMetaData)
            .Include(artwork => artwork.ArtworkCategories)
            .Select(artwork => new G2TopArtworkReadModel
            {
                Id = artwork.Id,
                Title = artwork.Title,
                Introduction = artwork.Introduction,
                ThumbnailUrl = artwork.ThumbnailUrl,
                OriginImageUrl = artwork.Origin.ImageUrl,
                ArtworkStatus = artwork.ArtworkStatus,
                FixedTotalChapters = artwork.FixedTotalChapters,
                LastChapterUploadOrder = artwork.LastChapterUploadOrder,
                Creator = new UserProfile
                {
                    UserId = artwork.CreatedBy,
                    AvatarUrl = artwork.Creator.AvatarUrl,
                    NickName = artwork.Creator.NickName,
                },
                ArtworkMetaData = new ArtworkMetaData
                {
                    TotalStarRates = artwork.ArtworkMetaData.TotalStarRates,
                    TotalUsersRated = artwork.ArtworkMetaData.TotalUsersRated,
                },
                ArtworkCategories = artwork.ArtworkCategories
                    .Select(artworkCategory => new Category
                    {
                        Name = artworkCategory.Category.Name,
                    }),
            })
            .AsSplitQuery()
            .ToDictionaryAsync(artwork => artwork.Id, cancellationToken);

        // Execute step 3: Get the latest chapter of each recommended artwork.
        foreach (var entry in candidateArtworkDictionary)
        {
            var currentArtwork = entry.Value;

            ArtworkChapter latestChapter = await GetLatestArtworkChapterCompiledQuery(
                arg1: _dbContext,
                arg2: currentArtwork.Id,
                arg3: currentArtwork.LastChapterUploadOrder);

            currentArtwork.LatestChapterId = latestChapter.Id;
            currentArtwork.LatestChapterTitle = latestChapter.Title;
        }

        return candidateArtworkDictionary.Values;
    }
}
