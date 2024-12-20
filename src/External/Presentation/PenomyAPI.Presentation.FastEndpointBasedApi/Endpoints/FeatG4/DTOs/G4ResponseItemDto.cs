using System.Collections.Generic;
using System.Linq;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG4;
using ProtoBuf;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG4.DTOs
{
    [ProtoContract]
    public class G4ResponseItemDto
    {
        [ProtoMember(1)]
        public string CategoryId { get; set; }

        [ProtoMember(2)]
        public string CategoryName { get; set; }

        [ProtoMember(3)]
        public IEnumerable<G4RecommendedComicResponseDto> RecommendedArtworks { get; set; }

        public static G4ResponseItemDto MapFrom(
            RecommendedArtworkByCategory recommendedArtworkByCategory
        )
        {
            var category = recommendedArtworkByCategory.Category;
            var recommendedComics = recommendedArtworkByCategory.RecommendedArtworks;

            return new()
            {
                CategoryId = category.Id.ToString(),
                CategoryName = category.Name,
                RecommendedArtworks = recommendedComics.Select(
                    G4RecommendedComicResponseDto.MapFrom
                ),
            };
        }

        public static IEnumerable<G4ResponseItemDto> MapFromList(
            IEnumerable<RecommendedArtworkByCategory> recommendedArtworkByCategories
        )
        {
            return recommendedArtworkByCategories.Select(MapFrom);
        }
    }
}
