using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG2;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using System.Collections.Generic;
using System.Linq;
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
        // ID candidate list.

        // Execute step 1:
        var candidateArtworks = _artworkDbSet
            .AsNoTracking()
            .Where(
                artwork => artwork.ArtworkType == artworkType
                && !artwork.IsTakenDown
                && !artwork.IsTemporarilyRemoved
                && artwork.PublicLevel == ArtworkPublicLevel.Everyone)
            .Select(artwork => new Artwork
            {
                Id = artwork.Id,
                ArtworkMetaData = new ArtworkMetaData
                {
                    TotalFollowers = artwork.ArtworkMetaData.TotalFollowers,
                    TotalFavorites = artwork.ArtworkMetaData.TotalFavorites,
                    TotalViews = artwork.ArtworkMetaData.TotalViews,
                    TotalStarRates = artwork.ArtworkMetaData.TotalStarRates,
                    TotalUsersRated = artwork.ArtworkMetaData.TotalUsersRated,
                },
            })
            .OrderByDescending(artwork => artwork.ArtworkMetaData.TotalFollowers)
            .ThenByDescending(artwork => artwork.ArtworkMetaData.TotalFavorites)
            .ThenByDescending(artwork => artwork.ArtworkMetaData.TotalViews)
            .ThenByDescending(artwork => artwork.ArtworkMetaData.TotalStarRates)
            .Take(MAXIMUM_NUMBER_OF_CANDIDATE_IDS);

        // After sort by criteria, then sort by star rate average.
        var candidateArtworkIds = await candidateArtworks
            .OrderByDescending(artwork
                => artwork.ArtworkMetaData.TotalStarRates / artwork.ArtworkMetaData.TotalUsersRated)
            .Take(NUMBER_OF_CANDIDATE_IDS_TO_BE_RETURNED)
            .Select(artwork => artwork.Id)
            .ToListAsync(cancellationToken);

        // title, artwork-type, origin, total-chapters, last chapter, categories list, star-rates, intro, thumbnail.
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

        var latestChapterOfEachArtworks = new List<ArtworkChapter>();

        foreach (var entry in candidateArtworkDictionary)
        {
            // Get latest chapter of each artwork.
            var currentArtwork = entry.Value;
            var artworkId = currentArtwork.Id;
            var lastChapterUploadOrder = currentArtwork.LastChapterUploadOrder;

            ArtworkChapter latestChapter = await _chapterDbSet
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
                .FirstOrDefaultAsync(cancellationToken);

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
