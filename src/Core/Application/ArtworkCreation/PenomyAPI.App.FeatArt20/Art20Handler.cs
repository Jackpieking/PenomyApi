using PenomyAPI.App.Common;
using PenomyAPI.App.Common.AppConstants;
using PenomyAPI.App.Common.FileServices.Models;
using PenomyAPI.App.Common.Helpers;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.App.FeatArt20.Infrastructures;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatArt20;

public class Art20Handler : IFeatureHandler<Art20Request, Art20Response>
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;
    private IArt20Repository _art20Repository;
    private readonly Lazy<ISnowflakeIdGenerator> _idGenerator;
    private readonly Lazy<IArt20FileService> _fileService;

    public Art20Handler(
        Lazy<IUnitOfWork> unitOfWork,
        Lazy<ISnowflakeIdGenerator> idGenerator,
        Lazy<IArt20FileService> fileService)
    {
        _unitOfWork = unitOfWork;
        _idGenerator = idGenerator;
        _fileService = fileService;
    }

    public async Task<Art20Response> ExecuteAsync(Art20Request request, CancellationToken cancellationToken)
    {
        var idGenerator = _idGenerator.Value;
        var chapterId = idGenerator.Get();

        var animeFolderName = request.AnimeId.ToString();
        var chapterFolderName = chapterId.ToString();

        // The relative path of the chapter folder will be: {comicId}/{chapterId}
        var chapterFolderInfo = new AppFolderInfo
        {
            RelativePath = DirectoryPathHelper.BuildPath(
                pathSeparator: DirectoryPathHelper.WebPathSeparator,
                rootDirectory: animeFolderName,
                subFolders: chapterFolderName
            )
        };

        var fileService = _fileService.Value;

        // Create the folder with provided info.
        var folderCreateResult = await fileService.CreateFolderAsync(
            folderInfo: chapterFolderInfo,
            cancellationToken: cancellationToken
        );

        if (!folderCreateResult)
        {
            return Art20Response.FILE_SERVICE_ERROR;
        }

        if (request.HasThumbnailFile())
        {
            var chapterThumbnailFileInfo = request.ThumbnailFileInfo;
            var thumbnailFilePrefix = ArtworkChapter.THUMBNAIL_IMAGE_FILE_PREFIX;

            chapterThumbnailFileInfo.FileId = thumbnailFilePrefix;
            chapterThumbnailFileInfo.FileName = $"{thumbnailFilePrefix}.{chapterThumbnailFileInfo.FileExtension}";
            chapterThumbnailFileInfo.FolderPath = chapterFolderInfo.AbsolutePath;

            var uploadFileResult = await fileService.UploadImageFileAsync(
                chapterThumbnailFileInfo,
                true,
                cancellationToken);

            if (!uploadFileResult.IsSuccess)
            {
                return Art20Response.FILE_SERVICE_ERROR;
            }
        }

        // Provide the file Id for uploaded video file to keep track and store in database.
        var videoMediaId = idGenerator.Get();

        request.ChapterVideoFileInfo.FileId = videoMediaId.ToString();
        request.ChapterVideoFileInfo.FolderPath = chapterFolderInfo.AbsolutePath;

        var videoUploadResult = await fileService.UploadVideoFileAsync(
            request.ChapterVideoFileInfo,
            true,
            cancellationToken);

        if (!videoUploadResult.IsSuccess)
        {
            return Art20Response.FILE_SERVICE_ERROR;
        }

        // Check if the chapter is created with drafted mode.
        var newChapterUploadOrder = 0;
        var unitOfWork = _unitOfWork.Value;

        if (request.PublishStatus == PublishStatus.Drafted)
        {
            newChapterUploadOrder = ArtworkChapter.DRAFTED_UPLOAD_ORDER;
            // If chapter is uploaded in drafted mode, then set the public level to private.
            request.PublicLevel = ArtworkPublicLevel.Private;
        }
        else
        {
            // Get the last chapter upload order of this comic, then the upload order
            // of the new chapter will be the last upload order increased by 1.
            var lastChapterUploadOrder = await unitOfWork.ArtworkRepository
                .GetLastChapterUploadOrderByArtworkIdAsync(
                    request.AnimeId,
                    cancellationToken);

            newChapterUploadOrder = lastChapterUploadOrder + 1;
        }

        // Save the chapter detail into the database.
        var dateTimeUtcNow = DateTime.UtcNow;
        var dateTimeMinUtc = CommonValues.DateTimes.MinUtc;

        var chapterDetail = new ArtworkChapter
        {
            Id = chapterId,
            ArtworkId = request.AnimeId,
            Title = request.Title,
            Description = request.Description ?? "None",
            CreatedAt = dateTimeUtcNow,
            CreatedBy = request.CreatedBy,
            UpdatedAt = dateTimeUtcNow,
            UpdatedBy = request.CreatedBy,
            IsTemporarilyRemoved = false,
            PublicLevel = request.PublicLevel,
            PublishStatus = request.PublishStatus,
            PublishedAt = request.PublishedAt,
            TemporarilyRemovedAt = dateTimeMinUtc,
            TemporarilyRemovedBy = request.CreatedBy,
            UploadOrder = newChapterUploadOrder,
            AllowComment = request.AllowComment,
        };

        // If thumbnail is specified for this chapter, then set the the thumbnail url.
        if (request.HasThumbnailFile())
        {
            chapterDetail.ThumbnailUrl = request.ThumbnailFileInfo.StorageUrl;
        }
        else
        {
            chapterDetail.ThumbnailUrl = await unitOfWork.ArtworkRepository
                .GetChapterThumbnailDefaultUrlByArtworkIdAsync(
                    request.AnimeId,
                    cancellationToken);
        }

        var chapterVideoMedia = new ArtworkChapterMedia
        {
            Id = videoMediaId,
            ChapterId = chapterId,
            FileName = $"{chapterId}.{request.ChapterVideoFileInfo.FileExtension}",
            FileSize = request.ChapterVideoFileInfo.FileSize,
            StorageUrl = request.ChapterVideoFileInfo.StorageUrl,
            MediaType = ArtworkChapterMediaType.Video,
            UploadOrder = 1,
        };

        _art20Repository = unitOfWork.Art20Repository;

        var result = await _art20Repository.CreateAnimeChapterAsync(
            chapterDetail,
            chapterVideoMedia,
            cancellationToken);

        if (!result)
        {
            return Art20Response.DATABASE_ERROR;
        }

        return Art20Response.SUCCESS;
    }
}
