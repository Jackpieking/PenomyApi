using PenomyAPI.App.Common;
using PenomyAPI.App.Common.FileServices;
using PenomyAPI.App.Common.FileServices.Models;
using PenomyAPI.App.Common.Helpers;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatArt17;

public class Art17Handler : IFeatureHandler<Art17Request, Art17Response>
{
    private const string AnimeFolder = "animations";

    private readonly Lazy<IUnitOfWork> _unitOfWork;
    private IArt17Repository _art17Repository;
    private readonly Lazy<IDefaultDistributedFileService> _fileService;

    public Art17Handler(
        Lazy<IUnitOfWork> unitOfWork,
        Lazy<IDefaultDistributedFileService> fileService
    )
    {
        _unitOfWork = unitOfWork;
        _fileService = fileService;
    }

    public async Task<Art17Response> ExecuteAsync(Art17Request request, CancellationToken ct)
    {
        _art17Repository = _unitOfWork.Value.Art17Repository;

        var isComicExisted = await _art17Repository.CheckCreatorPermissionAsync(
            request.AnimeId,
            request.UpdatedBy,
            ct
        );

        if (!isComicExisted)
        {
            return Art17Response.CURRENT_CREATOR_IS_NOT_AUTHORIZED;
        }

        Result<AppFileInfo> uploadThumbnailResult = Result<AppFileInfo>.Failed();

        if (request.IsThumbnailUpdated)
        {
            uploadThumbnailResult = await UploadAgainComicThumbnailAsync(request, ct);

            if (!uploadThumbnailResult.IsSuccess)
            {
                return Art17Response.FILE_SERVICE_ERROR;
            }
        }

        // Update the comic detail.
        var updatedAtUtcNow = DateTime.UtcNow;

        var updateDetail = new Artwork
        {
            Id = request.AnimeId,
            Title = request.Title,
            Introduction = request.Introduction,
            ThumbnailUrl = request.IsThumbnailUpdated
                ? uploadThumbnailResult.Value.StorageUrl
                : string.Empty,
            ArtworkOriginId = request.OriginId,
            ArtworkStatus = request.ArtworkStatus,
            PublicLevel = request.PublicLevel,
            AllowComment = request.AllowComment,
            UpdatedAt = updatedAtUtcNow,
        };

        // Update again the category list if any change is found.
        IEnumerable<ArtworkCategory> newCategories = null;

        if (request.IsCategoriesUpdated)
        {
            newCategories = request.ArtworkCategories;
        }

        var result = await _art17Repository.UpdateAnimeAsync(
            animeDetail: updateDetail,
            artworkCategories: newCategories,
            isThumbnailUpdated: request.IsThumbnailUpdated,
            isCategoriesUpdated: request.IsCategoriesUpdated,
            cancellationToken: ct
        );

        if (!result)
        {
            return Art17Response.DATABASE_ERROR;
        }

        return Art17Response.SUCCESS;
    }

    private async Task<Result<AppFileInfo>> UploadAgainComicThumbnailAsync(
        Art17Request request,
        CancellationToken ct
    )
    {
        // Upload the new thumbnail of the comic.
        var thumnailFileInfo = request.ThumbnailFileInfo;

        // The name of the artwork folder will be similar to the id of that artwork.
        var artworkFolderName = request.AnimeId.ToString();

        thumnailFileInfo.FolderPath = DirectoryPathHelper.BuildPath(
            pathSeparator: DirectoryPathHelper.WebPathSeparator,
            rootDirectory: AnimeFolder,
            subFolders: artworkFolderName
        );

        // Get the file service to upload the file.
        var fileService = _fileService.Value;

        var uploadFileResult = await fileService.UploadFileAsync(
            fileInfo: thumnailFileInfo,
            overwrite: true,
            cancellationToken: ct
        );

        return uploadFileResult;
    }
}
