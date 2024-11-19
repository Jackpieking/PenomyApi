using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.App.Common.AppConstants;
using PenomyAPI.App.Common.FileServices;
using PenomyAPI.App.Common.FileServices.Models;
using PenomyAPI.App.Common.Helpers;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using PenomyAPI.Infra.Configuration.Options;

namespace PenomyAPI.App.FeatArt4;

public class Art4Handler : IFeatureHandler<Art4Request, Art4Response>
{
    private readonly CloudinaryOptions _options;
    private readonly IArt4Repository _art4Repository;
    private readonly Lazy<IDefaultDistributedFileService> _fileService;

    public Art4Handler(
        Lazy<IUnitOfWork> unitOfWork,
        Lazy<IDefaultDistributedFileService> fileService,
        CloudinaryOptions options
    )
    {
        _art4Repository = unitOfWork.Value.Art4Repository;
        _fileService = fileService;
        _options = options;
    }

    public async Task<Art4Response> ExecuteAsync(Art4Request request, CancellationToken ct)
    {
        // The name of the artwork folder will be similar to the id of that artwork.
        var artworkFolderName = request.ComicId.ToString();

        // The info of folder to store the thumbnail of this comic.
        var folderInfo = new AppFolderInfo
        {
            RelativePath = DirectoryPathHelper.BuildPath(
                pathSeparator: DirectoryPathHelper.WebPathSeparator,
                rootDirectory: _options.ComicRootFolder,
                subFolders: artworkFolderName
            )
        };

        var fileService = _fileService.Value;

        // Create the folder with provided info.
        var folderCreateResult = await fileService.CreateFolderAsync(
            folderInfo: folderInfo,
            cancellationToken: ct
        );

        if (!folderCreateResult)
        {
            return new Art4Response
            {
                IsSuccess = false,
                ErrorMessages = ["Cannot create folder using file service"],
                StatusCode = Art4ResponseStatusCode.FILE_SERVICE_ERROR,
            };
        }

        // Upload the thumbnail of the comic.
        var thumnailFileInfo = request.ThumbnailFileInfo;
        thumnailFileInfo.FolderPath = folderInfo.RelativePath;

        var uploadFileResult = await fileService.UploadFileAsync(
            fileInfo: thumnailFileInfo,
            overwrite: true,
            cancellationToken: ct
        );

        // Get the storage url after upload the success.
        thumnailFileInfo.StorageUrl = uploadFileResult.Value.StorageUrl;

        // Last chapter order must be 0 because comic is created new.
        const int IntialOrder = 0;
        var dateTimeUtcNow = DateTime.UtcNow;
        var dateTimeMinUtc = CommonValues.DateTimes.MinUtc;

        var newComic = new Artwork
        {
            Id = request.ComicId,
            Title = request.Title,
            ArtworkType = ArtworkType.Comic,
            ArtworkOriginId = request.OriginId,
            ThumbnailUrl = thumnailFileInfo.StorageUrl,
            Introduction = request.Introduction,
            AuthorName = request.AuthorName,
            ArtworkStatus = ArtworkStatus.OnGoing,
            PublicLevel = request.PublicLevel,
            AllowComment = request.AllowComment,
            CreatedAt = dateTimeUtcNow,
            CreatedBy = request.CreatedBy,
            UpdatedAt = dateTimeUtcNow,
            UpdatedBy = request.CreatedBy,
            TemporarilyRemovedBy = request.CreatedBy,
            TemporarilyRemovedAt = dateTimeMinUtc,
            HasSeries = false,
            IsTakenDown = false,
            LastChapterUploadOrder = IntialOrder,
            IsCreatedByAuthorizedUser = false,
            IsTemporarilyRemoved = false,
        };

        var comicMetaData = ArtworkMetaData.Empty(request.ComicId);

        var comicCategories = request.ArtworkCategories;

        var result = await _art4Repository.CreateComicAsync(
            comic: newComic,
            comicMetaData: comicMetaData,
            artworkCategories: comicCategories,
            cancellationToken: ct
        );

        // If result is false.
        if (!result)
        {
            return new Art4Response
            {
                IsSuccess = false,
                StatusCode = Art4ResponseStatusCode.DATABASE_ERROR
            };
        }

        return new Art4Response
        {
            IsSuccess = true,
            ComicId = newComic.Id,
            StatusCode = Art4ResponseStatusCode.SUCCESS
        };
    }
}
