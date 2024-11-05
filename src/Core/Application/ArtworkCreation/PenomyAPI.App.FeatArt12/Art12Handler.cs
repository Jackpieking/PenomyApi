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

        // Check if the specified updated mode is valid or not
        // if the request body has flag (isDrafted = true).
        // to prevent the user from changing the chapter that been published into drafted mode.
        var currentUploadOrder = ArtworkChapter.DRAFTED_UPLOAD_ORDER;

        if (request.IsDrafted())
        {
            currentUploadOrder = await _art12Repository.GetCurrentUploadOrderByChapterIdAsync(
                request.ChapterId,
                cancellationToken);

            var isValidPublishMode = currentUploadOrder == ArtworkChapter.DRAFTED_UPLOAD_ORDER;

            if (!isValidPublishMode)
            {
                return Art12Response.INVALID_PUBLISH_STATUS;
            }
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

        // Set related data to update the chapter detail to the database.
        var updatedAt = DateTime.UtcNow;

        var chapterDetail = new ArtworkChapter
        {
            Id = request.ChapterId,
            Title = request.Title,
            Description = request.Description,
            AllowComment = request.AllowComment,
            PublicLevel = request.PublicLevel,
            PublishedAt = request.ScheduledAt,
            UpdatedAt = updatedAt,
            UpdatedBy = request.CreatorId,
        };

        // If upload in drafted mode then updated related info differently.
        if (request.IsDrafted())
        {
            chapterDetail.PublicLevel = ArtworkPublicLevel.Private;
            chapterDetail.PublishStatus = PublishStatus.Drafted;
        }
        else if (request.IsScheduled())
        {
            chapterDetail.PublicLevel = request.PublicLevel;
            chapterDetail.PublishStatus = PublishStatus.Scheduled;
            chapterDetail.PublishedAt = request.ScheduledAt;
        }
        else if (request.IsPublished())
        {
            chapterDetail.PublicLevel = request.PublicLevel;
            chapterDetail.PublishStatus = PublishStatus.Published;
        }

        // If not upload in drafted mode, check if the current upload order
        // is equal ArtworkChapter.DRAFTED_UPLOAD_ORDER or not
        // then update the upload order differently.
        var isChangedFromDrafted = !request.IsDrafted()
            && currentUploadOrder == ArtworkChapter.DRAFTED_UPLOAD_ORDER;

        // If this chapter is changed from drafted mode to other mode then resolve.
        if (isChangedFromDrafted)
        {
            // Get the last chapter upload order of this comic, then the upload order
            // of the new chapter will be the last upload order increased by 1.
            var lastChapterUploadOrder = await _unitOfWork.ArtworkRepository
                .GetLastChapterUploadOrderByArtworkIdAsync(
                    request.ComicId,
                    cancellationToken);

            chapterDetail.UploadOrder = lastChapterUploadOrder + 1;
        }
        else
        {
            chapterDetail.UploadOrder = currentUploadOrder;
        }

        // Update the related data into database.
        var updateResult = await _art12Repository.UpdateComicChapterAsync(
            changeFromDrafted: isChangedFromDrafted,
            updateContentOnly: request.IsUpdateContentOnly(),
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
}