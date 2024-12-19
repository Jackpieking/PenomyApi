using PenomyAPI.App.Common;
using PenomyAPI.App.Common.FileServices.Models;
using PenomyAPI.App.FeatArt22.Enums;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using System;

namespace PenomyAPI.App.FeatArt22;

public class Art22Request : IFeatureRequest<Art22Response>
{
    public long CreatorId { get; set; }

    public long ChapterId { get; set; }

    public long AnimeId { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public ArtworkPublicLevel PublicLevel { get; set; }

    public bool AllowComment { get; set; }

    public ChapterUpdateMode UpdateMode { get; set; }

    /// <summary>
    ///     Check if current chapter want to update to scheduled status or not.
    /// </summary>
    public bool IsChangedToSchedule()
    {
        return UpdateMode == ChapterUpdateMode.Scheduled;
    }

    /// <summary>
    ///     Check if current chapter want to update to published status or not.
    /// </summary>
    public bool IsChangeToPublish()
    {
        return UpdateMode == ChapterUpdateMode.Published;
    }

    /// <summary>
    ///     Check if current chapter just want to update its content only.
    /// </summary>
    /// <returns></returns>
    public bool IsUpdateContentOnly()
    {
        return UpdateMode == ChapterUpdateMode.UpdateContentOnly;
    }

    public DateTime ScheduledAt { get; set; }

    public ImageFileInfo ThumbnailFileInfo { get; set; }

    public string GetUpdatedThumbnailUrl()
    {
        if (Equals(ThumbnailFileInfo, null))
        {
            return null;
        }

        return ThumbnailFileInfo.StorageUrl;
    }

    /// <summary>
    ///     List of all newly upload chapter images file info.
    /// </summary>
    public VideoFileInfo NewChapterVideoFileInfo { get; set; }

    public bool HasThumbnailFile()
    {
        return !Equals(ThumbnailFileInfo, null);
    }

    public bool HasNewUploadChapterVideoFile()
    {
        return !Equals(NewChapterVideoFileInfo, null);
    }
}
