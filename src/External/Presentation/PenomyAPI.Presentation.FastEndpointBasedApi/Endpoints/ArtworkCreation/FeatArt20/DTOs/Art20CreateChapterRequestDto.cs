using Microsoft.AspNetCore.Http;
using PenomyAPI.App.Common.FileServices.Models;
using PenomyAPI.App.FeatArt20;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Helpers.IFormFiles;
using System;
using System.ComponentModel.DataAnnotations;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt20.DTOs;

public class Art20CreateChapterRequestDto
{
    [Required]
    public long AnimeId { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public IFormFile ThumbnailImageFile { get; set; }

    [Required]
    public IFormFile ChapterVideoFile { get; set; }

    public ArtworkPublicLevel PublicLevel { get; set; }

    public bool AllowComment { get; set; }

    public bool IsScheduled { get; set; }

    public bool IsDrafted { get; set; }

    public DateTime ScheduledAt { get; set; }

    public Art20Request MapToRequest(long creatorId)
    {
        PublishStatus publishStatus;

        // If is draft is set true, then status must be drafted, ignore the schedule time.
        if (IsDrafted)
        {
            publishStatus = PublishStatus.Drafted;
        }
        else
        {
            // If is scheduled is set true, then the publish status must be schedule, otherwise published.
            publishStatus = IsScheduled ? PublishStatus.Scheduled : PublishStatus.Published;
        }

        var request = new Art20Request
        {
            AnimeId = AnimeId,
            Title = Title,
            Description = Description,
            PublicLevel = PublicLevel,
            PublishStatus = publishStatus,
            CreatedBy = creatorId,
            PublishedAt = DateTime.SpecifyKind(ScheduledAt, DateTimeKind.Utc),
            AllowComment = AllowComment,
            ChapterVideoFileInfo = new VideoFileInfo
            {
                UploadOrder = 1,
                FileName = ChapterVideoFile.FileName,
                FileSize = ChapterVideoFile.Length,
                FileDataStream = ChapterVideoFile.OpenReadStream(),
                FileExtension = FormFileHelper.Instance.GetFileExtension(ChapterVideoFile),
            }
        };

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
