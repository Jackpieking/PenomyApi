using System;
using System.Collections.Generic;
using System.Linq;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt12.DTOs.GetChapterDetail;

public sealed class Art12GetChapterDetailResponseDto
{
    public string Id { get; set; }

    public string ComicId { get; set; }

    public string ComicTitle { get; set; }

    public string Title { get; set; }

    public string ThumbnailUrl { get; set; }

    public int UploadOrder { get; set; }

    public string Description { get; set; }

    public PublishStatus PublishStatus { get; set; }

    public ArtworkPublicLevel PublicLevel { get; set; }

    public bool AllowComment { get; set; }

    public DateTime PublishedAt { get; set; }

    public IEnumerable<Art12ChapterMediaItemResponseDto> ChapterMedias { get; set; }

    public long TotalMediaSize { get; set; }

    public static Art12GetChapterDetailResponseDto MapFrom(ArtworkChapter chapterDetail)
    {
        return new()
        {
            Id = chapterDetail.Id.ToString(),
            ComicId = chapterDetail.ArtworkId.ToString(),
            ComicTitle = chapterDetail.BelongedArtwork.Title,
            Title = chapterDetail.Title,
            ThumbnailUrl = chapterDetail.ThumbnailUrl,
            UploadOrder = chapterDetail.UploadOrder,
            Description = chapterDetail.Description,
            PublicLevel = chapterDetail.PublicLevel,
            PublishStatus = chapterDetail.PublishStatus,
            AllowComment = chapterDetail.AllowComment,
            PublishedAt = chapterDetail.PublishedAt,
            ChapterMedias = chapterDetail.ChapterMedias.Select(
                Art12ChapterMediaItemResponseDto.MapFrom
            ),
            TotalMediaSize = chapterDetail.ChapterMedias.Sum(media => media.FileSize)
        };
    }
}
