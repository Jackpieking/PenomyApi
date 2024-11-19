using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG3;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG4;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG4.DTOs
{
    public class G4NewChapterResponseDto
    {
        public string ChapterId { get; set; }

        public int UploadOrder { get; set; }

        public DateTime PublishedAt { get; set; }

        public static G4NewChapterResponseDto MapFrom(G4NewChapterReadModel chapterDetail)
        {
            return new()
            {
                ChapterId = chapterDetail.Id.ToString(),
                UploadOrder = chapterDetail.UploadOrder,
                PublishedAt = chapterDetail.PublishedAt,
            };
        }

        public static IEnumerable<G4NewChapterResponseDto> MapFromArray(
            IEnumerable<G4NewChapterReadModel> chapters)
        {
            return chapters.Select(MapFrom);
        }
    }
}
