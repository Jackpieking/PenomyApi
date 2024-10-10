using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.App.Common.FileServices;
using PenomyAPI.App.Common.FileServices.Models;
using PenomyAPI.App.Common.Helpers;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using PenomyAPI.Infra.Configuration.Options;

namespace PenomyAPI.App.FeatArt7;

public sealed class Art7Handler : IFeatureHandler<Art7Request, Art7Response>
{
    private readonly CloudinaryOptions _options;
    private readonly IArt7Repository _art7Repository;
    private readonly Lazy<IDefaultDistributedFileService> _fileService;

    public Art7Handler(
        Lazy<IUnitOfWork> unitOfWork,
        Lazy<IDefaultDistributedFileService> fileService,
        CloudinaryOptions options
    )
    {
        _art7Repository = unitOfWork.Value.Art7Repository;
        _fileService = fileService;
        _options = options;
    }

    public async Task<Art7Response> ExecuteAsync(Art7Request requestBody, CancellationToken ct)
    {
        var isComicExisted = await _art7Repository.IsComicExistedByIdAsync(
            comicId: requestBody.ComicId,
            cancellationToken: ct
        );

        if (!isComicExisted)
        {
            return Art7Response.ComicIdNotFound;
        }

        Result<AppFileInfo> uploadThumbnailResult = Result<AppFileInfo>.Failed();

        if (requestBody.IsThumbnailUpdated)
        {
            uploadThumbnailResult = await UploadAgainComicThumbnailAsync(requestBody, ct);

            if (!uploadThumbnailResult.IsSuccess)
            {
                return Art7Response.FileServiceError;
            }
        }

        // Update the comic detail.
        var updatedAtUtcNow = DateTime.UtcNow;

        var comicUpdateDetail = new Artwork
        {
            Id = requestBody.ComicId,
            Title = requestBody.Title,
            Introduction = requestBody.Introduction,
            ThumbnailUrl = requestBody.IsThumbnailUpdated
                ? uploadThumbnailResult.Value.StorageUrl
                : string.Empty,
            ArtworkOriginId = requestBody.OriginId,
            ArtworkStatus = requestBody.ArtworkStatus,
            PublicLevel = requestBody.PublicLevel,
            AllowComment = requestBody.AllowComment,
            UpdatedAt = updatedAtUtcNow,
        };

        IEnumerable<ArtworkCategory> comicNewCategories = null;

        if (requestBody.IsCategoriesUpdated)
        {
            comicNewCategories = requestBody.ArtworkCategories;
        }

        var result = await _art7Repository.UpdateComicAsync(
            comic: comicUpdateDetail,
            artworkCategories: comicNewCategories,
            isThumbnailUpdated: requestBody.IsThumbnailUpdated,
            isCategoriesUpdated: requestBody.IsCategoriesUpdated,
            cancellationToken: ct
        );

        if (!result)
        {
            return Art7Response.DatabaseError;
        }

        return new Art7Response { IsSuccess = true, StatusCode = Art7ResponseStatusCode.SUCCESS };
    }

    private async Task<Result<AppFileInfo>> UploadAgainComicThumbnailAsync(
        Art7Request request,
        CancellationToken ct
    )
    {
        // Upload the new thumbnail of the comic.
        var thumnailFileInfo = request.ThumbnailFileInfo;

        // The name of the artwork folder will be similar to the id of that artwork.
        var artworkFolderName = request.ComicId.ToString();

        thumnailFileInfo.FolderPath = DirectoryPathHelper.BuildPath(
            pathSeparator: DirectoryPathHelper.WebPathSeparator,
            rootDirectory: _options.ComicRootFolder,
            childFolders: artworkFolderName
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
