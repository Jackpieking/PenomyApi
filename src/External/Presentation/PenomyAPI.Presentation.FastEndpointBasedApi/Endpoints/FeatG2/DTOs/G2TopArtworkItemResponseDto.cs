using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG2;
using System.Collections.Generic;
using System.Linq;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG2.DTOs;

public sealed class G2TopArtworkItemResponseDto
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

    public string CreatorId { get; set; }

    public string CreatorName { get; set; }

    public string CreatorAvatarUrl { get; set; }

    public IEnumerable<string> Categories { get; set; }

    private static G2TopArtworkItemResponseDto MapFrom(G2TopArtworkReadModel artworkDetail)
    {
        return new G2TopArtworkItemResponseDto
        {
            Id = artworkDetail.Id.ToString(),
            Title = artworkDetail.Title,
            OriginImageUrl = artworkDetail.OriginImageUrl,
            ThumbnailUrl = artworkDetail.ThumbnailUrl,
            Introduction = artworkDetail.Introduction,
            LastChapterUploadOrder = artworkDetail.LastChapterUploadOrder,
            FixedTotalChapters = artworkDetail.FixedTotalChapters,
            // Latest chapter detail section
            LatestChapterId = artworkDetail.LatestChapterId.ToString(),
            LatestChapterTitle = artworkDetail.LatestChapterTitle,
            // Get the categories list
            Categories = artworkDetail.ArtworkCategories
                .Select(category => category.Name),
            // Calculate the average star rate
            AverageStarRates = artworkDetail.ArtworkMetaData.GetAverageStarRate(),
            CreatorId = artworkDetail.Creator.UserId.ToString(),
            CreatorName = artworkDetail.Creator.NickName,
            CreatorAvatarUrl = artworkDetail.Creator.AvatarUrl,
        };
    }

    /// <summary>
    ///     A short hand to map the input <see cref="G2TopArtworkReadModel"/>
    ///     to the list of <see cref="G2TopArtworkItemResponseDto"/> items.
    /// </summary>
    /// <param name="topArtworks"></param>
    /// <returns>
    ///     The list of <see cref="G2TopArtworkItemResponseDto"/> items after mapping.
    /// </returns>
    public static IEnumerable<G2TopArtworkItemResponseDto> MapFrom(
        ICollection<G2TopArtworkReadModel> topArtworks)
    {
        return topArtworks.Select(MapFrom);
    }
}
