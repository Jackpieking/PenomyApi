using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
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

    public static G3ResponseItemDto MapFrom(Artwork artworkDetail)
    {
        return new G3ResponseItemDto
        {
            ArtworkId = artworkDetail.Id.ToString(),
            Title = artworkDetail.Title,
            ThumbnailUrl = artworkDetail.ThumbnailUrl,
            OriginImageUrl = artworkDetail.Origin.ImageUrl,
            CreatorId = artworkDetail.Creator.UserId.ToString(),
            CreatorAvatarUrl = artworkDetail.Creator.AvatarUrl,
            CreatorName = artworkDetail.Creator.NickName,
            AverageStarRates = artworkDetail.ArtworkMetaData.GetAverageStarRate(),
            NewChapters = artworkDetail.Chapters.Select(G3ChapterItemResponseDto.MapFrom)
        };
    }
}