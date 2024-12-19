using Microsoft.AspNetCore.Http;
using PenomyAPI.App.Common.FileServices.Models;
using PenomyAPI.App.FeatArt22;
using PenomyAPI.App.FeatArt22.Enums;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Helpers.IFormFiles;
using System;
using System.ComponentModel.DataAnnotations;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt22.DTOs;

public class Art22UpdateAnimeChapterRequestDto
{
    [Required]
    public long AnimeId { get; set; }

    [Required]
    public long ChapterId { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public IFormFile ThumbnailImageFile { get; set; }

    public IFormFile ChapterVideoFile { get; set; }

    public ArtworkPublicLevel PublicLevel { get; set; }

    public bool AllowComment { get; set; }

    public ChapterUpdateMode UpdateMode { get; set; }

    public DateTime ScheduledAt { get; set; }

    public bool HasThumbnailImageFile() => ThumbnailImageFile != null;

    public bool HasChapterVideoFile() => ChapterVideoFile != null;

    public Art22Request MapToRequest(long creatorId)
    {
        var request = new Art22Request
        {
            AnimeId = AnimeId,
            ChapterId = ChapterId,
            CreatorId = creatorId,
            Title = Title,
            Description = Description,
            PublicLevel = PublicLevel,
            UpdateMode = UpdateMode,
            ScheduledAt = DateTime.SpecifyKind(ScheduledAt, DateTimeKind.Utc),
            AllowComment = AllowComment,
        };

        if (!Equals(ChapterVideoFile, null))
        {
            request.NewChapterVideoFileInfo = new VideoFileInfo
            {
                UploadOrder = 1,
                FileName = ChapterVideoFile.FileName,
                FileSize = ChapterVideoFile.Length,
                FileDataStream = ChapterVideoFile.OpenReadStream(),
                FileExtension = FormFileHelper.Instance.GetFileExtension(ChapterVideoFile),
            };
        }

        // Set the thumbnail image for the chapter (if have).
        if (!Equals(ThumbnailImageFile, null))
        {
            request.ThumbnailFileInfo = new ImageFileInfo
            {
                FileName = ThumbnailImageFile.FileName,
                FileSize = ThumbnailImageFile.Length,
                FileDataStream = ThumbnailImageFile.OpenReadStream(),
                FileExtension = FormFileHelper.Instance.GetFileExtension(ThumbnailImageFile)
            };
        }

        return request;
    }
}
