using System;
using System.Collections.Generic;
using System.Linq;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG4;
using ProtoBuf;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG4.DTOs
{
    [ProtoContract]
    public class G4NewChapterResponseDto
    {
        [ProtoMember(1)]
        public string ChapterId { get; set; }

        [ProtoMember(2)]
        public int UploadOrder { get; set; }

        [ProtoMember(3)]
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
            IEnumerable<G4NewChapterReadModel> chapters
        )
        {
            return chapters.Select(MapFrom);
        }
    }
}
