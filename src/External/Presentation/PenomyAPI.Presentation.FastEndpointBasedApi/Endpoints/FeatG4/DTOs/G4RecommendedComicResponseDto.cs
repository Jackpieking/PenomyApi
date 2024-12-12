using System.Collections.Generic;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG4;
using ProtoBuf;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG4.DTOs
{
    [ProtoContract]
    public class G4RecommendedComicResponseDto
    {
        /// <summary>
        ///     Id of the comic (artwork item).
        /// </summary>
        [ProtoMember(1)]
        public string ArtworkId { get; set; }

        [ProtoMember(2)]
        public string Title { get; set; }

        [ProtoMember(3)]
        public string ThumbnailUrl { get; set; }

        [ProtoMember(4)]
        public string OriginImageUrl { get; set; }

        [ProtoMember(5)]
        public double AverageStarRates { get; set; }

        [ProtoMember(6)]
        public string CreatorId { get; set; }

        [ProtoMember(7)]
        public string CreatorAvatarUrl { get; set; }

        [ProtoMember(8)]
        public string CreatorName { get; set; }

        [ProtoMember(9)]
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
