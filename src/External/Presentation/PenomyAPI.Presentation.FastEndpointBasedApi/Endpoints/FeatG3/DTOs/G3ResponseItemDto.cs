using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG3;
using System.Collections.Generic;
using System.Linq;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG3.DTOs;

public class G3ResponseItemDto
{
    public string ArtworkId { get; set; }

    public string Title { get; set; }

    public string ThumbnailUrl { get; set; }

    public string OriginImageUrl { get; set; }

    public double AverageStarRates { get; set; }

    public string CreatorId { get; set; }

    public string CreatorAvatarUrl { get; set; }

    public string CreatorName { get; set; }

    public IEnumerable<G3ChapterItemResponseDto> NewChapters { get; set; }

    public static G3ResponseItemDto MapFrom(RecentlyUpdatedComicReadModel artworkDetail)
    {
        return new G3ResponseItemDto
        {
            ArtworkId = artworkDetail.Id.ToString(),
            Title = artworkDetail.Title,
            ThumbnailUrl = artworkDetail.ThumbnailUrl,
            OriginImageUrl = artworkDetail.OriginImageUrl,
            CreatorId = artworkDetail.CreatorId.ToString(),
            CreatorAvatarUrl = artworkDetail.CreatorAvatarUrl,
            CreatorName = artworkDetail.CreatorName,
            AverageStarRates = artworkDetail.AverageStarRates,
            NewChapters = artworkDetail.NewChapters.Select(G3ChapterItemResponseDto.MapFrom)
        };
    }
}