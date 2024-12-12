using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG3;
using System;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG3.DTOs;

public class G3ChapterItemResponseDto
{
    public string ChapterId { get; set; }

    public int UploadOrder { get; set; }

    public DateTime PublishedAt { get; set; }

    public static G3ChapterItemResponseDto MapFrom(G3NewChapterReadModel chapterDetail)
    {
        return new G3ChapterItemResponseDto
        {
            ChapterId = chapterDetail.Id.ToString(),
            UploadOrder = chapterDetail.UploadOrder,
            PublishedAt = chapterDetail.PublishedAt,
        };
    }
}
