using PenomyAPI.App.Common;
using PenomyAPI.App.Common.AppConstants;
using PenomyAPI.App.Common.FileServices.Models;
using PenomyAPI.App.Common.Helpers;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.App.FeatArt10.Infrastructures;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatArt10;

public class Art10Handler : IFeatureHandler<Art10Request, Art10Response>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IArt10Repository _art10Repository;
    private readonly Lazy<ISnowflakeIdGenerator> _idGenerator;
    private readonly Lazy<IArt10FileService> _fileService;

    public Art10Handler(
        Lazy<ISnowflakeIdGenerator> idGenerator,
        Lazy<IUnitOfWork> unitOfWork,
        Lazy<IArt10FileService> fileService)
    {
        _idGenerator = idGenerator;
        _unitOfWork = unitOfWork.Value;
        _art10Repository = _unitOfWork.Art10Repository;
        _fileService = fileService;
    }

    public async Task<Art10Response> ExecuteAsync(Art10Request request, CancellationToken cancellationToken)
    {
        var idGenerator = _idGenerator.Value;
        var chapterId = idGenerator.Get();
        var comicFolderName = request.ComicId.ToString();
        var chapterFolderName = chapterId.ToString();

        // The relative path of the chapter folder will be: {comicId}/{chapterId}
        var chapterFolderInfo = new AppFolderInfo
        {
            FolderName = chapterFolderName,
            RelativePath = DirectoryPathHelper.BuildPath(
                pathSeparator: DirectoryPathHelper.WebPathSeparator,
                rootDirectory: comicFolderName,
                childFolders: chapterFolderName
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
            return Art10Response.FILE_SERVICE_ERROR;
        }

        foreach (var fileInfo in request.ChapterImageFileInfos)
        {
            fileInfo.FileId = idGenerator.Get().ToString();
            fileInfo.FolderPath = chapterFolderInfo.AbsolutePath;
        }

        // If the chapter has thumbnail image, then upload the thumbnail image with the chapter images.
        // The information of the thumbnail image and the chapter images will be updated after uploading successfully.
        var hasThumbnailImage = !Equals(request.ThumbnailFileInfo, null);
        if (hasThumbnailImage)
        {
            var chapterThumbnailFileInfo = request.ThumbnailFileInfo;
            var thumbnailFilePrefix = ArtworkChapter.THUMBNAIL_IMAGE_FILE_PREFIX;

            chapterThumbnailFileInfo.FileId = thumbnailFilePrefix;
            chapterThumbnailFileInfo.FileName = $"{thumbnailFilePrefix}.{chapterThumbnailFileInfo.FileExtension}";
            chapterThumbnailFileInfo.FolderPath = chapterFolderInfo.AbsolutePath;

            request.ChapterImageFileInfos.Add(request.ThumbnailFileInfo);
        }

        var uploadResult = await fileService.UploadMultipleFilesAsync(request.ChapterImageFileInfos, true, cancellationToken);

        if (!uploadResult.IsSuccess)
        {
            return Art10Response.FILE_SERVICE_ERROR;
        }

        // Remove the thumbnail image file info out of the ChapterImageFileInfos list
        // to prevent inserting into the chapter-medias table.
        var lastIndex = request.ChapterImageFileInfos.Count - 1;

        request.ChapterImageFileInfos.RemoveAt(lastIndex);

        // Check if the chapter is created with drafted mode.
        var newChapterUploadOrder = 0;

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
            var lastChapterUploadOrder = await _art10Repository.GetLastChapterUploadOrderByComicIdAsync(
                request.ComicId,
                cancellationToken);

            newChapterUploadOrder = lastChapterUploadOrder + 1;
        }

        // Save the chapter detail into the database.
        var dateTimeUtcNow = DateTime.UtcNow;
        var dateTimeMinUtc = CommonValues.DateTimes.MinUtc;

        var chapterDetail = new ArtworkChapter
        {
            Id = chapterId,
            ArtworkId = request.ComicId,
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
        if (hasThumbnailImage)
        {
            chapterDetail.ThumbnailUrl = request.ThumbnailFileInfo.StorageUrl;
        }
        else
        {
            chapterDetail.ThumbnailUrl = await _unitOfWork.ArtworkRepository
                .GetChapterThumbnailDefaultUrlByArtworkIdAsync(
                    request.ComicId,
                    cancellationToken);
        }

        // Get the list of chapter medias to save to database.
        IEnumerable<ArtworkChapterMedia> chapterMedias = null;

        try
        {
            chapterMedias = request.ChapterImageFileInfos.Select(chapterMedia => new ArtworkChapterMedia
            {
                Id = long.Parse(chapterMedia.FileId),
                ChapterId = chapterId,
                MediaType = ArtworkChapterMediaType.Image,
                FileName = chapterMedia.FileName,
                FileSize = chapterMedia.FileSize,
                StorageUrl = chapterMedia.StorageUrl,
                UploadOrder = chapterMedia.UploadOrder,
            });
        }
        catch
        {
            return Art10Response.FILE_SERVICE_ERROR;
        }

        var result = await _art10Repository.CreateComicChapterAsync(
            chapterDetail,
            chapterMedias,
            cancellationToken);

        if (!result)
        {
            return Art10Response.DATABASE_ERROR;
        }

        return Art10Response.SUCCESS;
    }
}
