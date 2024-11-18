using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG4;
using System.Collections.Generic;
using System.Linq;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG4.DTOs
{
    public class G4ResponseItemDto
    {
        public string CategoryId { get; set; }

        public string CategoryName { get; set; }

        public IEnumerable<G4RecommendedComicResponseDto> RecommendedArtworks { get; set; }

        public static G4ResponseItemDto MapFrom(RecommendedComicByCategory recommendedArtworkByCategory)
        {
            var category = recommendedArtworkByCategory.Category;
            var recommendedComics = recommendedArtworkByCategory.RecommendedComics;

            return new()
            {
                CategoryId = category.Id.ToString(),
                CategoryName = category.Name,
                RecommendedArtworks = recommendedComics.Select(G4RecommendedComicResponseDto.MapFrom)
            };
        }

        public static IEnumerable<G4ResponseItemDto> MapFromList(IEnumerable<RecommendedComicByCategory> recommendedArtworkByCategories)
        {
            return recommendedArtworkByCategories.Select(MapFrom);
        }
    }
}
