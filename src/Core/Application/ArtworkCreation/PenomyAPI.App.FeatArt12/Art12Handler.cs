using PenomyAPI.App.Common;
using PenomyAPI.App.Common.Helpers;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.App.FeatArt12.Infrastructures;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatArt12;

public sealed class Art12Handler
    : IFeatureHandler<Art12Request, Art12Response>
{
    // Readonly fields section.
    private readonly Lazy<ISnowflakeIdGenerator> _idGenerator;
    private readonly IArt12FileService _fileService;
    private readonly IArt12Repository _art12Repository;
    private readonly IUnitOfWork _unitOfWork;

    public Art12Handler(
        Lazy<IArt12FileService> fileService,
        Lazy<IUnitOfWork> unitOfWork,
        Lazy<ISnowflakeIdGenerator> idGenerator)
    {
        _fileService = fileService.Value;
        _unitOfWork = unitOfWork.Value;
        _art12Repository = _unitOfWork.Art12Repository;
        _idGenerator = idGenerator;
    }

    public async Task<Art12Response> ExecuteAsync(Art12Request request, CancellationToken cancellationToken)
    {
        // Validate the request detail before updating.
        // Checking the existence of the comic chapter.
        var isChapterExisted = await _art12Repository.IsComicChapterExistedAsync(
            request.ComicId,
            request.ChapterId,
            cancellationToken);

        if (!isChapterExisted)
        {
            return Art12Response.CHAPTER_IS_NOT_FOUND;
        }

        // Checking if the chapter is alreadly temporarily removed or not.
        var isChapterTemporarilyRemoved = await _art12Repository.IsChapterTemporarilyRemovedByIdAsync(
            request.ChapterId,
            cancellationToken);

        if (isChapterTemporarilyRemoved)
        {
            return Art12Response.CHAPTER_IS_TEMPORARILY_REMOVED;
        }

        // Checking if the current creator has permission to update the chapter.
        var hasPermissionToUpdate = await _art12Repository.HasPermissionToUpdateChapterDetailAsync(
            request.CreatorId,
            request.ChapterId,
            cancellationToken);

        if (!hasPermissionToUpdate)
        {
            return Art12Response.NO_PERMISSION_GRANTED_FOR_CURRENT_CREATOR;
        }

        // Upload new chapter images files, if have
        var hasFilesUpload = request.HasThumbnailFile()
            || request.HasNewUploadChapterImageFiles();

        if (hasFilesUpload)
        {
            // Upload all related files to the file storage.
            var chapterFolderRelativePath = GetChapterFolderRelativePath(request);

            var uploadFileResult = await InternalProcessUploadFileAsync(
                request,
                chapterFolderRelativePath,
                cancellationToken);

            if (!uploadFileResult)
            {
                return Art12Response.FILE_SERVICE_ERROR;
            }
        }

        // Init artwork chapter instance to update the detail.
        // Set related data to update the chapter detail to the database.
        var chapterDetail = new ArtworkChapter
        {
            Id = request.ChapterId,
            ArtworkId = request.ComicId,
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
        var currentChapterPublishStatus = await _art12Repository.GetCurrentChapterPublishStatusAsync(
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
    ///     Process to upload the file to the storage
    ///     and return the storageURLs to persistent in database.
    /// </summary>
    /// <param name="request">
    ///     Request that contains the list of files to upload.
    /// </param>
    /// <param name="chapterFolderRelativePath">
    ///     Relative path of the folder of current chapter.
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task<bool> InternalProcessUploadFileAsync(
        Art12Request request,
        string chapterFolderRelativePath,
        CancellationToken cancellationToken)
    {
        // Set the folder path for the new uploaded chapter image files.
        var idGenerator = _idGenerator.Value;

        if (request.HasNewUploadChapterImageFiles())
        {
            foreach (var fileItem in request.NewChapterImageFileInfos)
            {
                // Init new random fileId and set to image file item.
                fileItem.FileId = idGenerator.Get().ToString();
                fileItem.FolderPath = chapterFolderRelativePath;
            };
        }

        // Check if thumbnail file is uploaded new.
        if (request.HasThumbnailFile())
        {
            // Set some related information before uploading.
            request.ThumbnailFileInfo.FileId = ArtworkChapter.THUMBNAIL_IMAGE_FILE_PREFIX;
            request.ThumbnailFileInfo.FolderPath = chapterFolderRelativePath;

            // Init list for later file upload.
            request.InitNewChapterImageFileInfoList();

            // Add the thumbnail image file add last position in the list
            // and upload with the chapter image files.
            request.NewChapterImageFileInfos.Add(request.ThumbnailFileInfo);
        }

        var uploadResult = await _fileService.UploadMultipleFilesAsync(
            request.NewChapterImageFileInfos,
            true,
            cancellationToken);

        // After upload the file, remove the thumbnail image file
        // from the chapter image file list.
        if (request.HasThumbnailFile())
        {
            var lastIndex = request.NewChapterImageFileInfos.Count - 1;

            request.NewChapterImageFileInfos.RemoveAt(lastIndex);
        }

        return uploadResult.IsSuccess;
    }

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
    ///     A <see cref="Task{Art12Response}"/> contains result of updating.
    /// </returns>
    private async Task<Art12Response> UpdateChapterContentOnlyAsync(
        Art12Request request,
        ArtworkChapter chapterDetail,
        CancellationToken cancellationToken)
    {
        // Update the related data into database.
        var updateResult = await _art12Repository.UpdateComicChapterAsync(
            isChangedFromDraftedToOtherPublishStatus: false,
            isScheduleDateTimeChanged: false,
            chapterDetail: chapterDetail,
            updatedChapterMediaItems: request.UpdatedChapterMedias,
            deletedChapterMediaIds: request.DeletedChapterMediaIds,
            createdNewChapterMediaItems: request.CreatedNewComicChapterMediaItems,
            cancellationToken: cancellationToken);

        if (!updateResult)
        {
            return Art12Response.DATABASE_ERROR;
        }

        // Remove all chapter media items that marked as deleted.
        if (request.HasDeletedChapterMediaIdList())
        {
            _ = RemoveMarkDeletedFilesAsync(request, cancellationToken);
        }

        return Art12Response.SUCCESS;
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
    ///     A <see cref="Task{Art12Response}"/> contains result of updating.
    /// </returns>
    private async Task<Art12Response> UpdateFromDraftToDifferentPublishStatusAsync(
        Art12Request request,
        ArtworkChapter chapterDetail,
        CancellationToken cancellationToken)
    {
        // Get the last chapter upload order of this comic, then the upload order
        // of the new chapter will be the last upload order increased by 1.
        var lastChapterUploadOrderOfCurrentComic = await _unitOfWork.ArtworkRepository
            .GetLastChapterUploadOrderByArtworkIdAsync(
                request.ComicId,
                cancellationToken);

        chapterDetail.UploadOrder = lastChapterUploadOrderOfCurrentComic + 1;

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
        var updateResult = await _art12Repository.UpdateComicChapterAsync(
            isChangedFromDraftedToOtherPublishStatus: true,
            isScheduleDateTimeChanged: true,
            chapterDetail: chapterDetail,
            updatedChapterMediaItems: request.UpdatedChapterMedias,
            deletedChapterMediaIds: request.DeletedChapterMediaIds,
            createdNewChapterMediaItems: request.CreatedNewComicChapterMediaItems,
            cancellationToken: cancellationToken);

        if (!updateResult)
        {
            return Art12Response.DATABASE_ERROR;
        }

        return Art12Response.SUCCESS;
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
    ///     A <see cref="Task{Art12Response}"/> contains result of updating.
    /// </returns>
    private async Task<Art12Response> UpdateChapterWithChangeInPublishDetailAsync(
        Art12Request request,
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
        var updateResult = await _art12Repository.UpdateComicChapterAsync(
            isChangedFromDraftedToOtherPublishStatus: false,
            isScheduleDateTimeChanged: true,
            chapterDetail: chapterDetail,
            updatedChapterMediaItems: request.UpdatedChapterMedias,
            deletedChapterMediaIds: request.DeletedChapterMediaIds,
            createdNewChapterMediaItems: request.CreatedNewComicChapterMediaItems,
            cancellationToken: cancellationToken);

        if (!updateResult)
        {
            return Art12Response.DATABASE_ERROR;
        }

        // Remove all chapter media items that marked as deleted.
        if (request.HasDeletedChapterMediaIdList())
        {
            _ = RemoveMarkDeletedFilesAsync(request, cancellationToken);
        }

        return Art12Response.SUCCESS;
    }

    /// <summary>
    ///     Remove the files that are marked as deleted from the storage.
    /// </summary>
    /// <param name="request">
    ///     The request contains the list of files that are marked as deleted.
    /// </param>
    /// <param name="cancellationToken"></param>
    private async Task<bool> RemoveMarkDeletedFilesAsync(
        Art12Request request,
        CancellationToken cancellationToken)
    {
        var chapterFolderRelativePath = GetChapterFolderRelativePath(request);

        return await _fileService.DeleteMultipleFileByIdsAsync(
            chapterFolderRelativePath,
            fileIds: request.DeletedChapterMediaIds,
            cancellationToken: cancellationToken);
    }

    private static string GetChapterFolderRelativePath(Art12Request request)
    {
        // Upload all related files to the file storage.
        var comicFolderName = request.ComicId.ToString();
        var chapterFolderName = request.ChapterId.ToString();

        var chapterFolderRelativePath = DirectoryPathHelper.BuildPath(
            pathSeparator: DirectoryPathHelper.WebPathSeparator,
            rootDirectory: comicFolderName,
            childFolders: chapterFolderName);

        return chapterFolderRelativePath;
    }
    #endregion
}