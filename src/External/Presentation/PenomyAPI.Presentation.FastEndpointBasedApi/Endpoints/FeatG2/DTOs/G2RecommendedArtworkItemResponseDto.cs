using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG2;
using System.Collections.Generic;
using System.Linq;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG2.DTOs;

public sealed class G2RecommendedArtworkItemResponseDto
{
    public string Id { get; set; }

    public string Title { get; set; }

    public string Origin { get; set; }

    public string OriginImageUrl { get; set; }

    public string ThumbnailUrl { get; set; }

    public string Introduction { get; set; }

    public int LastChapterUploadOrder { get; set; }

    public int FixedTotalChapters { get; set; }

    public string LatestChapterId { get; set; }

    public string LatestChapterTitle { get; set; }

    public double AverageStarRates { get; set; }

    public IEnumerable<string> Categories { get; set; }

    private static G2RecommendedArtworkItemResponseDto MapFrom(
        Artwork artworkDetail,
        ArtworkChapter latestChapterDetail)
    {
        return new G2RecommendedArtworkItemResponseDto
        {
            Id = artworkDetail.Id.ToString(),
            Title = artworkDetail.Title,
            Origin = artworkDetail.Origin.CountryName,
            OriginImageUrl = artworkDetail.Origin.ImageUrl,
            ThumbnailUrl = artworkDetail.ThumbnailUrl,
            Introduction = artworkDetail.Introduction,
            LastChapterUploadOrder = artworkDetail.LastChapterUploadOrder,
            FixedTotalChapters = artworkDetail.FixedTotalChapters,
            // Latest chapter detail section
            LatestChapterId = latestChapterDetail.Id.ToString(),
            LatestChapterTitle = latestChapterDetail.Title,
            // Get the categories list
            Categories = artworkDetail.ArtworkCategories
                .Select(artworkCategory => artworkCategory.Category.Name),
            // Calculate the average star rate
            AverageStarRates = artworkDetail.ArtworkMetaData.GetAverageStarRate()
        };
    }

    /// <summary>
    ///     A short hand to map the input <see cref="G2TopRecommendedArtworks"/>
    ///     to the list of <see cref="G2RecommendedArtworkItemResponseDto"/> items.
    /// </summary>
    /// <param name="topRecommendedArtworks"></param>
    /// <returns>
    ///     The list of <see cref="G2RecommendedArtworkItemResponseDto"/> items after mapping.
    /// </returns>
    public static IEnumerable<G2RecommendedArtworkItemResponseDto> MapFrom(
        G2TopRecommendedArtworks topRecommendedArtworks)
    {
        return topRecommendedArtworks.LatestChapterOfEachArtworks
            .Select(
                selector: latestChapter => MapFrom(
                    artworkDetail: latestChapter.BelongedArtwork,
                    latestChapterDetail: latestChapter));
    }
}
