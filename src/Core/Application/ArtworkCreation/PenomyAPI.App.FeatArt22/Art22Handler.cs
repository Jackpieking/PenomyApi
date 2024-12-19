using PenomyAPI.App.Common;
using PenomyAPI.App.Common.Helpers;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.App.FeatArt22.Infrastructures;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatArt22;

public class Art22Handler : IFeatureHandler<Art22Request, Art22Response>
{
    // Readonly fields section.
    private readonly Lazy<ISnowflakeIdGenerator> _idGenerator;
    private readonly IArt22FileService _fileService;
    private readonly IArt22Repository _art22Repository;
    private readonly IUnitOfWork _unitOfWork;

    public Art22Handler(
        Lazy<IArt22FileService> fileService,
        Lazy<IUnitOfWork> unitOfWork,
        Lazy<ISnowflakeIdGenerator> idGenerator)
    {
        _fileService = fileService.Value;
        _unitOfWork = unitOfWork.Value;
        _art22Repository = _unitOfWork.Art22Repository;
        _idGenerator = idGenerator;
    }

    public async Task<Art22Response> ExecuteAsync(Art22Request request, CancellationToken cancellationToken)
    {
        // Checking if the chapter is alreadly temporarily removed or not.
        var isChapterTemporarilyRemoved = await _art22Repository.IsChapterTemporarilyRemovedByIdAsync(
            request.ChapterId,
            cancellationToken);

        if (isChapterTemporarilyRemoved)
        {
            return Art22Response.CHAPTER_IS_TEMPORARILY_REMOVED;
        }

        // Checking if the current creator has permission to update the chapter.
        var hasPermissionToUpdate = await _art22Repository.HasPermissionToUpdateChapterDetailAsync(
            request.CreatorId,
            request.ChapterId,
            cancellationToken);

        if (!hasPermissionToUpdate)
        {
            return Art22Response.NO_PERMISSION_GRANTED_FOR_CURRENT_CREATOR;
        }

        // Upload again the thumbnail image file if not empty.
        if (request.HasThumbnailFile())
        {
            // Upload all related files to the file storage.
            var chapterFolderRelativePath = GetChapterFolderRelativePath(request);

            request.ThumbnailFileInfo.FileId = ArtworkChapter.THUMBNAIL_IMAGE_FILE_PREFIX;
            request.ThumbnailFileInfo.FolderPath = chapterFolderRelativePath;

            var uploadFileResult = await _fileService.UploadImageFileAsync(
                request.ThumbnailFileInfo,
                true,
                cancellationToken);

            if (!uploadFileResult.IsSuccess)
            {
                return Art22Response.FILE_SERVICE_ERROR;
            }
        }

        if (request.HasNewUploadChapterVideoFile())
        {
            // Upload all related files to the file storage.
            var chapterFolderRelativePath = GetChapterFolderRelativePath(request);

            request.NewChapterVideoFileInfo.FileId = request.ChapterId.ToString();
            request.NewChapterVideoFileInfo.FolderPath = chapterFolderRelativePath;

            var uploadFileResult = await _fileService.UploadVideoFileAsync(
                request.NewChapterVideoFileInfo,
                true,
                cancellationToken);

            if (!uploadFileResult.IsSuccess)
            {
                return Art22Response.FILE_SERVICE_ERROR;
            }
        }

        // Init artwork chapter instance to update the detail.
        // Set related data to update the chapter detail to the database.
        var chapterDetail = new ArtworkChapter
        {
            Id = request.ChapterId,
            ArtworkId = request.AnimeId,
            Title = request.Title,
            Description = request.Description,
            ThumbnailUrl = request.GetUpdatedThumbnailUrl(),
            PublicLevel = request.PublicLevel,
            AllowComment = request.AllowComment,
            UpdatedAt = DateTime.UtcNow,
            UpdatedBy = request.CreatorId,
        };

        // If creator only need to update content of the chapter, then resolve.
        if (request.IsUpdateContentOnly())
        {
            return await UpdateChapterContentOnlyAsync(
                request,
                chapterDetail,
                cancellationToken);
        }

        // If move to this step, the chapter must fall into 2 situations:
        // 1. From draft to other publish status.
        // 2. To update from Scheduled publish status to other publish status OR
        // update again the Schedule date.

        // Then get the current chapter publish status to resolve the update.
        var currentChapterPublishStatus = await _art22Repository.GetCurrentChapterPublishStatusAsync(
            request.ChapterId,
            cancellationToken);

        // If current publish status is drafted,
        // then the creator want to publish the draft with other publish status.
        var isChangedFromDrafted =
            currentChapterPublishStatus == PublishStatus.Drafted;

        // If this chapter is changed from drafted mode
        // to other mode then update its upload order.
        if (isChangedFromDrafted)
        {
            return await UpdateFromDraftToDifferentPublishStatusAsync(
                request,
                chapterDetail,
                cancellationToken);
        }

        // If not fall to any above update situation, then current chapter
        // must has scheduled status and need to update the schedule detail
        // or to change to published status.
        return await UpdateChapterWithChangeInPublishDetailAsync(
            request,
            currentChapterPublishStatus,
            chapterDetail,
            cancellationToken);
    }

    #region Private Methods
    /// <summary>
    ///     Update the current chapter's content only
    ///     without affecting its publish status or schedule time.
    /// </summary>
    /// <param name="request">
    ///     Request contains related information supported for updating.
    /// </param>
    /// <param name="chapterDetail">
    ///     ArtworkChapter instance contains information to update.
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    ///     A <see cref="Task{Art22Response}"/> contains result of updating.
    /// </returns>
    private async Task<Art22Response> UpdateChapterContentOnlyAsync(
        Art22Request request,
        ArtworkChapter chapterDetail,
        CancellationToken cancellationToken)
    {
        // Update the related data into database.
        var updateResult = await _art22Repository.UpdateAnimeChapterAsync(
            isChangedFromDraftedToOtherPublishStatus: false,
            isScheduleDateTimeChanged: false,
            chapterDetail: chapterDetail,
            chapterVideoMedia: null,
            cancellationToken: cancellationToken);

        if (!updateResult)
        {
            return Art22Response.DATABASE_ERROR;
        }

        return Art22Response.SUCCESS;
    }

    /// <summary>
    ///     Update the current chapter's that changed from 
    ///     draft status to different publish status.
    /// </summary>
    /// <param name="request">
    ///     Request contains related information supported for updating.
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    ///     A <see cref="Task{Art22Response}"/> contains result of updating.
    /// </returns>
    private async Task<Art22Response> UpdateFromDraftToDifferentPublishStatusAsync(
        Art22Request request,
        ArtworkChapter chapterDetail,
        CancellationToken cancellationToken)
    {
        // Get the last chapter upload order of this comic, then the upload order
        // of the new chapter will be the last upload order increased by 1.
        var lastChapterUploadOrderOfCurrentArtwork = await _unitOfWork.ArtworkRepository
            .GetLastChapterUploadOrderByArtworkIdAsync(
                request.AnimeId,
                cancellationToken);

        chapterDetail.UploadOrder = lastChapterUploadOrderOfCurrentArtwork + 1;

        // Update related info differently based on the update mode.
        if (request.IsChangedToSchedule())
        {
            chapterDetail.PublishStatus = PublishStatus.Scheduled;
            chapterDetail.PublishedAt = request.ScheduledAt;
        }
        else if (request.IsChangeToPublish())
        {
            chapterDetail.PublishStatus = PublishStatus.Published;
            chapterDetail.PublishedAt = chapterDetail.UpdatedAt;
        }

        // Update the related data into database.
        ArtworkChapterMedia chapterVideoMedia = null;

        if (request.NewChapterVideoFileInfo != null)
        {
            chapterVideoMedia = new()
            {
                ChapterId = request.ChapterId,
                StorageUrl = request.NewChapterVideoFileInfo.StorageUrl,
            };
        }

        var updateResult = await _art22Repository.UpdateAnimeChapterAsync(
            isChangedFromDraftedToOtherPublishStatus: true,
            isScheduleDateTimeChanged: true,
            chapterDetail: chapterDetail,
            chapterVideoMedia: chapterVideoMedia,
            cancellationToken: cancellationToken);

        if (!updateResult)
        {
            return Art22Response.DATABASE_ERROR;
        }

        return Art22Response.SUCCESS;
    }

    /// <summary>
    ///     Update the current chapter's that has any change 
    ///     in publish detail (publish datetime or publish status).
    /// </summary>
    /// <param name="request">
    ///     Request contains related information supported for updating.
    /// </param>
    /// <param name="currentChapterPublishStatus">
    ///     The publish status of current chapter supported for updating.
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    ///     A <see cref="Task{Art22Response}"/> contains result of updating.
    /// </returns>
    private async Task<Art22Response> UpdateChapterWithChangeInPublishDetailAsync(
        Art22Request request,
        PublishStatus currentChapterPublishStatus,
        ArtworkChapter chapterDetail,
        CancellationToken cancellationToken)
    {
        // Update schedule datetime based on the current chapter publish status.
        var isScheduleDateTimeChanged = request.IsChangedToSchedule()
            && currentChapterPublishStatus == PublishStatus.Scheduled;

        if (isScheduleDateTimeChanged)
        {
            chapterDetail.PublishStatus = PublishStatus.Scheduled;
            chapterDetail.PublishedAt = request.ScheduledAt;
        }
        else if (request.IsChangeToPublish())
        {
            chapterDetail.PublishStatus = PublishStatus.Published;
            chapterDetail.PublishedAt = chapterDetail.UpdatedAt;
        }

        // Update the related data into database.
        ArtworkChapterMedia chapterVideoMedia = null;

        if (request.NewChapterVideoFileInfo != null)
        {
            chapterVideoMedia = new()
            {
                ChapterId = request.ChapterId,
                StorageUrl = request.NewChapterVideoFileInfo.StorageUrl,
                FileSize = request.NewChapterVideoFileInfo.FileSize,
            };
        }

        var updateResult = await _art22Repository.UpdateAnimeChapterAsync(
            isChangedFromDraftedToOtherPublishStatus: false,
            isScheduleDateTimeChanged: true,
            chapterDetail: chapterDetail,
            chapterVideoMedia: chapterVideoMedia,
            cancellationToken: cancellationToken);

        if (!updateResult)
        {
            return Art22Response.DATABASE_ERROR;
        }

        return Art22Response.SUCCESS;
    }

    private static string GetChapterFolderRelativePath(Art22Request request)
    {
        // Upload all related files to the file storage.
        var comicFolderName = request.AnimeId.ToString();
        var chapterFolderName = request.ChapterId.ToString();

        var chapterFolderRelativePath = DirectoryPathHelper.BuildPath(
            pathSeparator: DirectoryPathHelper.WebPathSeparator,
            rootDirectory: comicFolderName,
            subFolders: chapterFolderName);

        return chapterFolderRelativePath;
    }
    #endregion
}
