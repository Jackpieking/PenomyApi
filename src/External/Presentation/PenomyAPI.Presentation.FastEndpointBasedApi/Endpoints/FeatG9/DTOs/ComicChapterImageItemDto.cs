using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG9.DTOs;

public sealed class ComicChapterImageItemDto
{
    public int UploadOrder { get; set; }

    public string StorageUrl { get; set; }

    public static ComicChapterImageItemDto MapFrom(ArtworkChapterMedia chapterImage)
    {
        return new()
        {
            UploadOrder = chapterImage.UploadOrder,
            StorageUrl = chapterImage.StorageUrl,
        };
    }
}
