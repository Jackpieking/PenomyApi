using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.App.Common.FileServices;
using PenomyAPI.App.Common.FileServices.Models;
using PenomyAPI.App.Common.Helpers;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using PenomyAPI.Infra.Configuration.Options;

namespace PenomyAPI.App.SM8;

public class SM8Handler : IFeatureHandler<SM8Request, SM8Response>
{
    private readonly ISM8Repository _SM8Repository;
    private readonly Lazy<ISnowflakeIdGenerator> _idGenerator;
    private readonly Lazy<IDefaultDistributedFileService> _fileService;
    private readonly CloudinaryOptions _options;

    public SM8Handler(
        Lazy<IUnitOfWork> unitOfWork,
        Lazy<ISnowflakeIdGenerator> idGenerator,
        CloudinaryOptions options,
        Lazy<IDefaultDistributedFileService> fileService
    )
    {
        _SM8Repository = unitOfWork.Value.SM8Repository;
        _idGenerator = idGenerator;
        _fileService = fileService;
        _options = options;
    }

    public async Task<SM8Response> ExecuteAsync(SM8Request request, CancellationToken ct)
    {
        try
        {
            var generatedId = _idGenerator.Value.Get();

            // Set image file Id
            request.CoverImageFileInfo.FileId = generatedId.ToString();
            // The name of the artwork folder will be similar to the id of that artwork.
            var artworkFolderName = generatedId.ToString();

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

            // Create the folder with provided info.
            var folderCreateResult = await fileService.CreateFolderAsync(
                folderInfo: folderInfo,
                cancellationToken: ct
            );

            if (!folderCreateResult)
            {
                return new SM8Response
                {
                    IsSuccess = false,
                    Message = ["Cannot create folder using file service"],
                    StatusCode = SM8ResponseStatusCode.FAILED,
                };
            }

            // Upload the thumbnail of the comic.
            var thumnailFileInfo = request.CoverImageFileInfo;
            thumnailFileInfo.FolderPath = folderInfo.RelativePath;

            var uploadFileResult = await fileService.UploadFileAsync(
                fileInfo: thumnailFileInfo,
                overwrite: true,
                cancellationToken: ct
            );

            // Get the storage url after upload the success.
            thumnailFileInfo.StorageUrl = uploadFileResult.Value.StorageUrl;

            var socialGroup = new SocialGroup()
            {
                Id = generatedId,
                Name = request.GroupName,
                IsPublic = request.IsPublic,
                Description = request.Description,
                CoverPhotoUrl = thumnailFileInfo.StorageUrl,
                RequireApprovedWhenPost = request.RequireApprovedWhenPost,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = long.Parse(request.GetUserId()),
                GroupStatus = SocialGroupStatus.Active,
                TotalMembers = 1,
            };

            long createdId = await _SM8Repository.CreateSocialGroupAsync(socialGroup);

            if (createdId == 0)
                return new SM8Response
                {
                    IsSuccess = false,
                    StatusCode = SM8ResponseStatusCode.FAILED,
                };
            return new SM8Response
            {
                IsSuccess = true,
                Result = createdId,
                StatusCode = SM8ResponseStatusCode.SUCCESS,
            };
        }
        catch
        {
            return new SM8Response { IsSuccess = false, StatusCode = SM8ResponseStatusCode.FAILED };
        }
    }
}
