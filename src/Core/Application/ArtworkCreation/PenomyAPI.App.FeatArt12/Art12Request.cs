using PenomyAPI.App.Common;
using PenomyAPI.App.Common.FileServices.Models;
using PenomyAPI.App.FeatArt12.Enums;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PenomyAPI.App.FeatArt12;

public sealed class Art12Request : IFeatureRequest<Art12Response>
{
    public long CreatorId { get; set; }

    public long ChapterId { get; set; }

    public long ComicId { get; set; }

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

    public AppFileInfo ThumbnailFileInfo { get; set; }

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
    public IList<AppFileInfo> NewChapterImageFileInfos { get; set; }

    /// <summary>
    ///     The list of chapter media items to updated.
    /// </summary>
    public IList<ArtworkChapterMedia> UpdatedChapterMedias { get; set; }

    /// <summary>
    ///     The list of chapter media IDs to deleted.
    /// </summary>
    public IList<long> DeletedChapterMediaIds { get; set; }

    public void InitUpdatedChapterMediaList()
    {
        if (Equals(UpdatedChapterMedias, null))
        {
            UpdatedChapterMedias = new List<ArtworkChapterMedia>();
        }
    }

    public void InitDeletedChapterMediaIdList()
    {
        if (Equals(DeletedChapterMediaIds, null))
        {
            DeletedChapterMediaIds = new List<long>();
        }
    }

    public void InitNewChapterImageFileInfoList()
    {
        if (Equals(NewChapterImageFileInfos, null))
        {
            NewChapterImageFileInfos = new List<AppFileInfo>();
        }
    }

    public bool HasThumbnailFile()
    {
        return !Equals(ThumbnailFileInfo, null);
    }

    public bool HasNewUploadChapterImageFiles()
    {
        return !Equals(NewChapterImageFileInfos, null);
    }

    public bool HasDeletedChapterMediaIdList()
    {
        return !Equals(DeletedChapterMediaIds, null);
    }

    #region Get CreatedNewComicChapterMediaItems
    private IEnumerable<ArtworkChapterMedia> _createdNewComicChapterMediaItems;

    public IEnumerable<ArtworkChapterMedia> CreatedNewComicChapterMediaItems
    {
        get
        {
            if (Equals(NewChapterImageFileInfos, null))
            {
                return null;
            }
            else
            {
                _createdNewComicChapterMediaItems = NewChapterImageFileInfos.Select(chapterMedia => new ArtworkChapterMedia
                {
                    Id = long.Parse(chapterMedia.FileId),
                    ChapterId = ChapterId,
                    MediaType = ArtworkChapterMediaType.Image,
                    FileName = chapterMedia.FileName,
                    FileSize = chapterMedia.FileSize,
                    UploadOrder = chapterMedia.UploadOrder,
                    StorageUrl = chapterMedia.StorageUrl,
                });
            }

            return _createdNewComicChapterMediaItems;
        }
    }
    #endregion
}
