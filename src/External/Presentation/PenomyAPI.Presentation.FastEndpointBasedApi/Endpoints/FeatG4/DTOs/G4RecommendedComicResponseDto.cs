using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG4;
using System.Collections.Generic;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG4.DTOs
{
    public class G4RecommendedComicResponseDto
    {
        /// <summary>
        ///     Id of the comic (artwork item).
        /// </summary>
        public string ArtworkId { get; set; }

        public string Title { get; set; }

        public string ThumbnailUrl { get; set; }

        public string OriginImageUrl { get; set; }

        public double AverageStarRates { get; set; }

        public string CreatorId { get; set; }

        public string CreatorAvatarUrl { get; set; }

        public string CreatorName { get; set; }

        public IEnumerable<G4NewChapterResponseDto> NewChapters { get; set; }

        public static G4RecommendedComicResponseDto MapFrom(RecommendedComicReadModel artworkDetail)
        {
            return new()
            {
                ArtworkId = artworkDetail.Id.ToString(),
                Title = artworkDetail.Title,
                ThumbnailUrl = artworkDetail.ThumbnailUrl,
                AverageStarRates = artworkDetail.AverageStarRates,
                OriginImageUrl = artworkDetail.OriginImageUrl,
                CreatorId = artworkDetail.CreatorId.ToString(),
                CreatorAvatarUrl = artworkDetail.CreatorAvatarUrl,
                CreatorName = artworkDetail.CreatorName,
                NewChapters = G4NewChapterResponseDto.MapFromArray(artworkDetail.NewChapters)
            };
        }
    }
}
