using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using System;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt6.DTOs;

public class ComicChapterDto
{
    public string Id { get; set; }

    public string Title { get; set; }

    public int UploadOrder { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime PublishedAt { get; set; }

    public PublishStatus PublishStatus { get; set; }

    public ArtworkPublicLevel PublicLevel { get; set; }

    public string ThumbnailUrl { get; set; }

    public bool AllowComment { get; set; }

    public static ComicChapterDto MapFrom(ArtworkChapter chapter)
    {
        return new()
        {
            Id = chapter.Id.ToString(),
            Title = chapter.Title,
            UploadOrder = chapter.UploadOrder,
            AllowComment = chapter.AllowComment,
            CreatedAt = chapter.CreatedAt,
            PublishedAt = chapter.PublishedAt,
            PublishStatus = chapter.PublishStatus,
            ThumbnailUrl = chapter.ThumbnailUrl,
            PublicLevel = chapter.PublicLevel,
        };
    }
}
