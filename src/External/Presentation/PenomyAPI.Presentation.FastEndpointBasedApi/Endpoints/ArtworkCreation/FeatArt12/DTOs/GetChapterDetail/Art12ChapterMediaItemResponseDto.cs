using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt12.DTOs.GetChapterDetail;

public sealed class Art12ChapterMediaItemResponseDto
{
    public string Id { get; set; }

    public int UploadOrder { get; set; }

    public long FileSize { get; set; }

    public string StorageUrl { get; set; }

    public static Art12ChapterMediaItemResponseDto MapFrom(ArtworkChapterMedia chapterMedia)
    {
        return new()
        {
            Id = chapterMedia.Id.ToString(),
            UploadOrder = chapterMedia.UploadOrder,
            FileSize = chapterMedia.FileSize,
            StorageUrl = chapterMedia.StorageUrl,
        };
    }
}
