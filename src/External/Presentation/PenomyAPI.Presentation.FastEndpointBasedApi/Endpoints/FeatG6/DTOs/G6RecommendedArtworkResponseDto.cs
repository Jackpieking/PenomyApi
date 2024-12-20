using System.Collections.Generic;
using System.Linq;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG6.DTOs;

public class G6RecommendedArtworkResponseDto
{
    public string Id { get; set; }

    public string Title { get; set; }

    public string ThumbnailUrl { get; set; }

    public IEnumerable<string> Categories { get; set; }

    public string AuthorName { get; set; }

    public static G6RecommendedArtworkResponseDto MapFrom(Artwork artworkDetail)
    {
        return new()
        {
            Id = artworkDetail.Id.ToString(),
            Title = artworkDetail.Title,
            ThumbnailUrl = artworkDetail.ThumbnailUrl,
            AuthorName = artworkDetail.Creator.NickName,
            Categories = artworkDetail.ArtworkCategories.Select(category => category.Category.Name)
        };
    }
}
