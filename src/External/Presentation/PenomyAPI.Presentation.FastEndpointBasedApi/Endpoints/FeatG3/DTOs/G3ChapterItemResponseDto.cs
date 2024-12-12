using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using System;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG3.DTOs;

public class G3ChapterItemResponseDto
{
    public string ChapterId { get; set; }

    public int UploadOrder { get; set; }

    public DateTime PublishedAt { get; set; }

    public static G3ChapterItemResponseDto MapFrom(ArtworkChapter chapterDetail)
    {
        return new G3ChapterItemResponseDto
        {
            ChapterId = chapterDetail.Id.ToString(),
            UploadOrder = chapterDetail.UploadOrder,
            PublishedAt = chapterDetail.PublishedAt,
        };
    }
}
