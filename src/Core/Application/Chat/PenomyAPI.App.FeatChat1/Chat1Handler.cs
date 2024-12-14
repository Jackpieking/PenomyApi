using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.App.Common.FileServices;
using PenomyAPI.App.Common.FileServices.Models;
using PenomyAPI.App.Common.Helpers;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.Domain.RelationalDb.DataSeedings.Roles;
using PenomyAPI.Domain.RelationalDb.Entities.Chat;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Chat;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using PenomyAPI.Infra.Configuration.Options;

namespace PenomyAPI.App.FeatChat1;

public class Chat1Handler : IFeatureHandler<Chat1Request, Chat1Response>
{
    private readonly IFeatChat1Repository _Chat1Repository;
    private readonly Lazy<IDefaultDistributedFileService> _fileService;
    private readonly Lazy<ISnowflakeIdGenerator> _idGenerator;
    private readonly CloudinaryOptions _options;

    public Chat1Handler(
        Lazy<IUnitOfWork> unitOfWork,
        Lazy<ISnowflakeIdGenerator> idGenerator,
        CloudinaryOptions options,
        Lazy<IDefaultDistributedFileService> fileService
    )
    {
        _Chat1Repository = unitOfWork.Value.FeatChat1Repository;
        _idGenerator = idGenerator;
        _fileService = fileService;
        _options = options;
    }

    public async Task<Chat1Response> ExecuteAsync(Chat1Request request, CancellationToken ct)
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
                    DirectoryPathHelper.WebPathSeparator,
                    "chatgroups",
                    artworkFolderName
                )
            };

            var fileService = _fileService.Value;

            // Create the folder with provided info.
            var folderCreateResult = await fileService.CreateFolderAsync(
                folderInfo,
                ct
            );

            if (!folderCreateResult)
                return new Chat1Response
                {
                    IsSuccess = false,
                    Message = ["Cannot create folder using file service"],
                    StatusCode = Chat1ResponseStatusCode.FAILED
                };

            // Upload the thumbnail of the comic.
            var thumnailFileInfo = request.CoverImageFileInfo;
            thumnailFileInfo.FolderPath = folderInfo.RelativePath;

            var uploadFileResult = await fileService.UploadFileAsync(
                thumnailFileInfo,
                true,
                ct
            );

            // Get the storage url after upload the success.
            thumnailFileInfo.StorageUrl = uploadFileResult.Value.StorageUrl;

            var socialGroup = new ChatGroup
            {
                Id = generatedId,
                GroupName = request.GroupName,
                IsPublic = request.IsPublic,
                CoverPhotoUrl = thumnailFileInfo.StorageUrl,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = request.UserId,
                ChatGroupType = request.GroupType,
                TotalMembers = 1
            };
            var chatOwner = new ChatGroupMember
            {
                MemberId = request.UserId,
                ChatGroupId = generatedId,
                RoleId = UserRoles.GroupManager.Id
            };
            var success = await _Chat1Repository.CreateGroupAsync(socialGroup, chatOwner, ct);

            if (!success)
                return new Chat1Response
                {
                    IsSuccess = false,
                    StatusCode = Chat1ResponseStatusCode.FAILED
                };
            return new Chat1Response
            {
                IsSuccess = true,
                Result = generatedId,
                StatusCode = Chat1ResponseStatusCode.SUCCESS
            };
        }
        catch
        {
            return new Chat1Response { IsSuccess = false, StatusCode = Chat1ResponseStatusCode.FAILED };
        }
    }
}
