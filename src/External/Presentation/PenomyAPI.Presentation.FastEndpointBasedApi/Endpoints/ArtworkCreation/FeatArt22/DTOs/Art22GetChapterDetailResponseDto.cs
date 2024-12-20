using PenomyAPI.App.FeatArt22.OtherHandlers.GetChapterDetail;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using System;
using System.Linq;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt22.DTOs;

public class Art22GetChapterDetailResponseDto
{
    public string Id { get; set; }

    public string AnimeId { get; set; }

    public string AnimeTitle { get; set; }

    public string Title { get; set; }

    public string ThumbnailUrl { get; set; }

    public int UploadOrder { get; set; }

    public string Description { get; set; }

    public PublishStatus PublishStatus { get; set; }

    public ArtworkPublicLevel PublicLevel { get; set; }

    public bool AllowComment { get; set; }

    public DateTime PublishedAt { get; set; }

    public string ChapterVideoUrl { get; set; }

    public string TotalMediaSize { get; set; }

    public static Art22GetChapterDetailResponseDto MapFrom(Art22GetChapterDetailResponse response)
    {
        var chapterDetail = response.ChapterDetail;

        return new()
        {
            Id = chapterDetail.Id.ToString(),
            AnimeId = chapterDetail.ArtworkId.ToString(),
            AnimeTitle = chapterDetail.BelongedArtwork.Title,
            Title = chapterDetail.Title,
            ThumbnailUrl = chapterDetail.ThumbnailUrl,
            UploadOrder = chapterDetail.UploadOrder,
            Description = chapterDetail.Description,
            PublicLevel = chapterDetail.PublicLevel,
            PublishStatus = chapterDetail.PublishStatus,
            AllowComment = chapterDetail.AllowComment,
            PublishedAt = chapterDetail.PublishedAt,
            ChapterVideoUrl = chapterDetail.ChapterMedias.FirstOrDefault().StorageUrl,
            TotalMediaSize = chapterDetail.ChapterMedias.Sum(media => media.FileSize).ToString()
        };
    }
}
