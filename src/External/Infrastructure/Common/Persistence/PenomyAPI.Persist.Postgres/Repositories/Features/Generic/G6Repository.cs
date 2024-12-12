using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Common.ExtensionMethods;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public class G6Repository : IG6Repository
{
    private const int TOTAL_RANDOM_CATEGORIES = 3;
    private const int TOTAL_DISPLAY_CATEGORIES = 2;
    private readonly DbSet<Artwork> _artworkDbSet;
    private readonly DbSet<ArtworkCategory> _artworkCategoryDbSet;

    public G6Repository(DbContext dbContext)
    {
        _artworkDbSet = dbContext.Set<Artwork>();
        _artworkCategoryDbSet = dbContext.Set<ArtworkCategory>();
    }

    public async Task<List<Artwork>> GetRecommendedArtworksAsync(
        long artworkId,
        int totalRecommendedArtworks,
        CancellationToken cancellationToken
    )
    {
        var currentArtwork = await _artworkDbSet
            .AsNoTracking()
            .Where(artwork => artwork.Id == artworkId)
            .Select(artwork => new Artwork
            {
                ArtworkType = artwork.ArtworkType,
                ArtworkCategories = artwork.ArtworkCategories.Select(category => new ArtworkCategory
                {
                    CategoryId = category.CategoryId
                })
            })
            .FirstOrDefaultAsync(cancellationToken);

        // Get random categories for later recommendation.
        var totalRandomCategories = TOTAL_RANDOM_CATEGORIES;
        var currentArtworkTotalCategories = currentArtwork.ArtworkCategories.Count();

        if (currentArtworkTotalCategories < TOTAL_RANDOM_CATEGORIES)
        {
            totalRandomCategories = currentArtworkTotalCategories;
        }

        var randomCategories = currentArtwork
            .ArtworkCategories.GetRandomElements(totalRandomCategories)
            .Select(category => category.CategoryId);

        // Get the recommended artwork ids for later retrieve information.
        var takeNumbers = TOTAL_RANDOM_CATEGORIES * totalRecommendedArtworks;

        var relatedArtworkIds = await _artworkCategoryDbSet
            .Where(artworkCategory =>
                // Get the artwork with similar artwork type.
                artworkCategory.Artwork.ArtworkType == currentArtwork.ArtworkType
                // Take the artworks that different with the current artwork.
                && artworkCategory.ArtworkId != artworkId
                && !artworkCategory.Artwork.IsTemporarilyRemoved
                && !artworkCategory.Artwork.IsTakenDown
                && artworkCategory.Artwork.PublicLevel == ArtworkPublicLevel.Everyone
                && randomCategories.Contains(artworkCategory.CategoryId)
            )
            .OrderByDescending(artworkCategory =>
                artworkCategory.Artwork.ArtworkMetaData.TotalStarRates
            )
            .Select(artworkCategory => artworkCategory.ArtworkId)
            .Take(takeNumbers)
            .ToListAsync(cancellationToken);

        var recommendedArtworkIds = relatedArtworkIds
            .Distinct()
            .GetRandomElements(totalRecommendedArtworks);

        List<Artwork> recommendedArtworkList = await _artworkDbSet
            .AsNoTracking()
            .Where(artwork => recommendedArtworkIds.Contains(artwork.Id))
            .Select(artwork => new Artwork
            {
                Id = artwork.Id,
                Title = artwork.Title,
                ThumbnailUrl = artwork.ThumbnailUrl,
                Creator = new UserProfile { NickName = artwork.Creator.NickName, },
                ArtworkCategories = artwork
                    .ArtworkCategories.Select(artworkCategory => new ArtworkCategory
                    {
                        Category = new Category { Name = artworkCategory.Category.Name }
                    })
                    .Take(TOTAL_DISPLAY_CATEGORIES)
            })
            .ToListAsync(cancellationToken);

        return recommendedArtworkList;
    }
}
