using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG19;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG19.DTOs;

public class G19ChapterItemResponseDto
{
    public string Id { get; set; }

    public int UploadOrder { get; set; }

    public string ThumbnailUrl { get; set; }

    public string ChapterName { get; set; }

    public static G19ChapterItemResponseDto MapFrom(G19AnimeChapterItemReadModel chapterItem)
    {
        return new()
        {
            Id = chapterItem.Id.ToString(),
            UploadOrder = chapterItem.UploadOrder,
            ChapterName = chapterItem.ChapterName,
            ThumbnailUrl = chapterItem.ThumbnailUrl,
        };
    }
}
