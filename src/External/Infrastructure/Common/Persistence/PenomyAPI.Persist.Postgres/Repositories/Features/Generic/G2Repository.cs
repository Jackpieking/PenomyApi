using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
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
    private const int MAXIMUM_NUMBER_OF_CANDIDATE_IDS = 10;
    private const int NUMBER_OF_CANDIDATE_IDS_TO_BE_RETURNED = 8;

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
    private static FormattableString GetCandidateArtworkIdsComicType = null;
    private static FormattableString GetCandidateArtworkIdsAnimationType = null;

    private static FormattableString GetCandidateArtworkIdsRawQuery(ArtworkType artworkType)
    {
        // If artwork type is comic, then cache the rawQuery and reused.
        lock (_lock)
        {
            if (
                Equals(GetCandidateArtworkIdsComicType, null)
                || Equals(GetCandidateArtworkIdsAnimationType, null)
            )
            {
                // Set up for comic type.
                GetCandidateArtworkIdsComicType = $@"
                    SELECT metadata_and_artwork.""Id""
                    FROM (
                          SELECT artwork.""Id"", metadata.""TotalStarRates"", metadata.""TotalUsersRated""
                          FROM penomy_artwork AS artwork
                          INNER JOIN penomy_artwork_metadata AS metadata ON artwork.""Id"" = metadata.""ArtworkId""
                          WHERE artwork.""ArtworkType"" = {ArtworkType.Comic} AND NOT (artwork.""IsTakenDown"") AND NOT (artwork.""IsTemporarilyRemoved"") AND artwork.""PublicLevel"" = {ArtworkPublicLevel.Everyone}
                          ORDER BY metadata.""TotalFollowers"" DESC, metadata.""TotalFavorites"" DESC, metadata.""TotalViews"" DESC, metadata.""TotalStarRates"" DESC
                          LIMIT {MAXIMUM_NUMBER_OF_CANDIDATE_IDS}
                    ) AS metadata_and_artwork
                    ORDER BY metadata_and_artwork.""TotalStarRates"" / metadata_and_artwork.""TotalUsersRated"" DESC
                    LIMIT {NUMBER_OF_CANDIDATE_IDS_TO_BE_RETURNED}";

                // Set up for animation type.
                GetCandidateArtworkIdsAnimationType = $@"
                    SELECT metadata_and_artwork.""Id""
                    FROM (
                          SELECT artwork.""Id"", metadata.""TotalStarRates"", metadata.""TotalUsersRated""
                          FROM penomy_artwork AS artwork
                          INNER JOIN penomy_artwork_metadata AS metadata ON artwork.""Id"" = metadata.""ArtworkId""
                          WHERE artwork.""ArtworkType"" = {ArtworkType.Animation} AND NOT (artwork.""IsTakenDown"") AND NOT (artwork.""IsTemporarilyRemoved"") AND artwork.""PublicLevel"" = {ArtworkPublicLevel.Everyone}
                          ORDER BY metadata.""TotalFollowers"" DESC, metadata.""TotalFavorites"" DESC, metadata.""TotalViews"" DESC, metadata.""TotalStarRates"" DESC
                          LIMIT {MAXIMUM_NUMBER_OF_CANDIDATE_IDS}
                    ) AS metadata_and_artwork
                    ORDER BY metadata_and_artwork.""TotalStarRates"" / metadata_and_artwork.""TotalUsersRated"" DESC
                    LIMIT {NUMBER_OF_CANDIDATE_IDS_TO_BE_RETURNED}";
            }
        }

        if (artworkType == ArtworkType.Comic)
        {
            return GetCandidateArtworkIdsComicType;
        }
        else
        {
            return GetCandidateArtworkIdsAnimationType;
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
                        && (chapter.UploadOrder == lastChapterUploadOrder
                            || chapter.UploadOrder <= lastChapterUploadOrder)
                        && chapter.PublicLevel == ArtworkPublicLevel.Everyone
                        && chapter.PublishStatus == PublishStatus.Published)
                    .Select(chapter => new ArtworkChapter
                    {
                        Id = chapter.Id,
                        Title = chapter.Title,
                    })
                    .FirstOrDefault();

            GetLatestArtworkChapterCompiledQuery = EF.CompileAsyncQuery(GetLatestArtworkChapterExpression);

            // Set the flag to prevent set up multiple time.
            HasSetUp = true;

            // Clear the expression after setup success.
            GetLatestArtworkChapterExpression = null;
        }
    }
    #endregion

    public async Task<G2TopRecommendedArtworks> GetTopRecommendedArtworksByTypeAsync(
        ArtworkType artworkType,
        CancellationToken cancellationToken)
    {
        // The filter process have 2 steps:
        // + Step 1:
        //  a. Filter the list of 10 candidate IDs from the artwork-metadata table
        // based on the filter criteria: TotalFollow, TotalFavorites, AverageStarRate.
        // 
        //  b. After getting the filter IDs list, only get 8 of them to display (If the return list
        //  has less than 8 items, then return all).
        //
        // + Step 2: Filter from the artwork table all the artwork with the specified IDs in the
        // list of candidate IDs.
        // 
        // + Step 3: Get the detail of each artwork, including their latest chapter.

        // Set up the compilded queries before execution.
        SetUpCompiledQueries();

        // Execute step 1:
        var getCandidateArtworkIdsRawQuery = GetCandidateArtworkIdsRawQuery(artworkType);

        var candidateArtworkIds = await _dbContext.Database
            .SqlQuery<long>(getCandidateArtworkIdsRawQuery)
            .ToListAsync(cancellationToken);

        // Execute step 2:
        var candidateArtworkDictionary = await _artworkDbSet
            .AsNoTracking()
            .Where(artwork => candidateArtworkIds.Contains(artwork.Id))
            .Include(artwork => artwork.Origin)
            .Include(artwork => artwork.ArtworkMetaData)
            .Include(artwork => artwork.ArtworkCategories)
            .Select(artwork => new Artwork
            {
                Id = artwork.Id,
                Title = artwork.Title,
                Introduction = artwork.Introduction,
                ThumbnailUrl = artwork.ThumbnailUrl,
                Origin = new ArtworkOrigin
                {
                    Label = artwork.Origin.Label,
                    ImageUrl = artwork.Origin.ImageUrl,
                },
                FixedTotalChapters = artwork.FixedTotalChapters,
                LastChapterUploadOrder = artwork.LastChapterUploadOrder,
                ArtworkMetaData = new ArtworkMetaData
                {
                    TotalStarRates = artwork.ArtworkMetaData.TotalStarRates,
                    TotalUsersRated = artwork.ArtworkMetaData.TotalUsersRated,
                },
                ArtworkCategories = artwork.ArtworkCategories
                    .Select(artworkCategory => new ArtworkCategory
                    {
                        Category = new Category
                        {
                            Name = artworkCategory.Category.Name,
                        }
                    }),
            })
            .AsSplitQuery()
            .ToDictionaryAsync(artwork => artwork.Id, cancellationToken);

        // Execute step 3:
        var latestChapterOfEachArtworks = new List<ArtworkChapter>();

        foreach (var entry in candidateArtworkDictionary)
        {
            // Get latest chapter of each artwork.
            var currentArtwork = entry.Value;
            var artworkId = currentArtwork.Id;
            var lastChapterUploadOrder = currentArtwork.LastChapterUploadOrder;

            ArtworkChapter latestChapter = await GetLatestArtworkChapterCompiledQuery(
                arg1: _dbContext,
                arg2: artworkId,
                arg3: lastChapterUploadOrder);

            if (!Equals(latestChapter, null))
            {
                latestChapter.BelongedArtwork = currentArtwork;
                latestChapterOfEachArtworks.Add(latestChapter);
            }
        }

        return new G2TopRecommendedArtworks
        {
            LatestChapterOfEachArtworks = latestChapterOfEachArtworks,
            RecommendedArtworks = candidateArtworkDictionary.Values
        };
    }
}
