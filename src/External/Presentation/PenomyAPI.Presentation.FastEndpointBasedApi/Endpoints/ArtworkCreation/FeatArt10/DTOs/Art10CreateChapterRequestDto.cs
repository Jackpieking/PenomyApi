using Microsoft.AspNetCore.Http;
using PenomyAPI.App.Common.FileServices.Models;
using PenomyAPI.App.FeatArt10;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Helpers.IFormFiles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt10.DTOs;

public class Art10CreateChapterRequestDto
{
    [Required]
    public long ComicId { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    [Required]
    public IFormFile ThumbnailImageFile { get; set; }

    [Required]
    public IFormFileCollection ChapterImageFiles { get; set; }

    public ArtworkPublicLevel PublicLevel { get; set; }

    public bool AllowComment { get; set; }

    public bool IsScheduled { get; set; }

    public bool IsDrafted { get; set; }

    public DateTime ScheduledAt { get; set; }

    public Art10Request MapToRequest(long creatorId)
    {
        var publishStatus = PublishStatus.Published;

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

        var request = new Art10Request
        {
            ComicId = ComicId,
            Title = Title,
            Description = Description,
            PublicLevel = PublicLevel,
            PublishStatus = publishStatus,
            CreatedBy = creatorId,
            PublishedAt = DateTime.SpecifyKind(ScheduledAt, DateTimeKind.Utc),
            AllowComment = AllowComment,
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

        // Set the list of chapter image file infos.
        var chapterImageFileInfos = new List<AppFileInfo>();

        for (var index = 0; index < ChapterImageFiles.Count; index++)
        {
            var chatperImageFile = ChapterImageFiles[index];

            var chapterImageFileInfo = new ImageFileInfo
            {
                UploadOrder = index,
                FileDataStream = chatperImageFile.OpenReadStream(),
                FileSize = chatperImageFile.Length,
                FileName = chatperImageFile.FileName,
            };

            chapterImageFileInfos.Add(chapterImageFileInfo);
        }

        request.ChapterImageFileInfos = chapterImageFileInfos;

        return request;
    }
}