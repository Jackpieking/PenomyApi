using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using ProtoBuf;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG9.DTOs;

[ProtoContract]
public sealed class ComicChapterImageItemDto
{
    [ProtoMember(1)]
    public int UploadOrder { get; set; }

    [ProtoMember(2)]
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
