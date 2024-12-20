using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG9;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG9.DTOs;

public class G9ChapterItemResponseDto
{
    public string Id { get; set; }

    public int UploadOrder { get; set; }

    public string ThumbnailUrl { get; set; }

    public string ChapterName { get; set; }

    public static G9ChapterItemResponseDto MapFrom(G9ChapterItemReadModel chapterItem)
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
