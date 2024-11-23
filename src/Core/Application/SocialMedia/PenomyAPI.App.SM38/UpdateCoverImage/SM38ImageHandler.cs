using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.App.Common.FileServices;
using PenomyAPI.App.Common.FileServices.Models;
using PenomyAPI.App.Common.Helpers;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using PenomyAPI.Infra.Configuration.Options;

namespace PenomyAPI.App.SM38.CoverImage;

public class SM38ImageHandler : IFeatureHandler<SM38ImageRequest, SM38ImageResponse>
{
    private readonly ISM38Repository _SM38Repository;
    private readonly Lazy<IDefaultDistributedFileService> _fileService;
    private readonly CloudinaryOptions _options;

    public SM38ImageHandler(
        Lazy<IUnitOfWork> unitOfWork,
        CloudinaryOptions options,
        Lazy<IDefaultDistributedFileService> fileService
    )
    {
        _SM38Repository = unitOfWork.Value.SM38Repository;
        _fileService = fileService;
        _options = options;
    }

    public async Task<SM38ImageResponse> ExecuteAsync(
        SM38ImageRequest request,
        CancellationToken ct
    )
    {
        try
        {
            // Set image file Id
            request.CoverImageFileInfo.FileId = request.GroupId.ToString();
            // The name of the artwork folder will be similar to the id of that artwork.
            var artworkFolderName = request.CoverImageFileInfo.FileId;

            // The info of folder to store the cover iamge of this comic.
            var folderInfo = new AppFolderInfo
            {
                RelativePath = DirectoryPathHelper.BuildPath(
                    pathSeparator: DirectoryPathHelper.WebPathSeparator,
                    rootDirectory: "groups",
                    subFolders: artworkFolderName
                ),
            };

            var fileService = _fileService.Value;

            // Upload the cover image of the group.
            var coverImageFileInfo = request.CoverImageFileInfo;
            coverImageFileInfo.FolderPath = folderInfo.RelativePath;

            var uploadFileResult = await fileService.UploadFileAsync(
                fileInfo: coverImageFileInfo,
                overwrite: true,
                cancellationToken: ct
            );

            // Get the storage url after upload the success.
            coverImageFileInfo.StorageUrl = uploadFileResult.Value.StorageUrl;

            var Result = await _SM38Repository.UpdateGroupCoverPhotoAsync(
                request.UserId,
                request.GroupId,
                request.CoverImageFileInfo.StorageUrl
            );

            if (Result == 0)
                return new SM38ImageResponse
                {
                    StatusCode = SM38ResponseStatusCode.DATABSE_ERROR,
                    Result = "",
                    IsSuccess = false,
                };
            return new SM38ImageResponse
            {
                StatusCode = SM38ResponseStatusCode.SUCCESS,
                Result = request.CoverImageFileInfo.StorageUrl,
                IsSuccess = true,
            };
        }
        catch
        {
            return new SM38ImageResponse
            {
                IsSuccess = false,
                StatusCode = SM38ResponseStatusCode.FAILED,
            };
        }
    }
}
