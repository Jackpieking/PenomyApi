using PenomyAPI.App.Common;
using PenomyAPI.App.Common.FileServices;
using PenomyAPI.App.Common.Helpers;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatG36;

public sealed class G36Handler
    : IFeatureHandler<G36Request, G36Response>
{
    private const string ROOT_USER_AVATAR_FOLDER = "users";

    private readonly Lazy<IUnitOfWork> _unitOfWork;
    private IG36Repository _g36Repository;
    private readonly Lazy<IDefaultDistributedFileService> _fileService;

    public G36Handler(
        Lazy<IUnitOfWork> unitOfWork,
        Lazy<IDefaultDistributedFileService> fileService)
    {
        _unitOfWork = unitOfWork;
        _fileService = fileService;
    }

    public async Task<G36Response> ExecuteAsync(G36Request request, CancellationToken ct)
    {
        _g36Repository = _unitOfWork.Value.G36Repository;

        var isNickNameAlreadyExisted = await _g36Repository.IsNickNameAlreadyExistedAsync(
            request.UserId,
            request.NickName,
            ct);

        if (isNickNameAlreadyExisted)
        {
            return G36Response.NICKNAME_IS_ALREADY_EXISTED;
        }

        string newAvatarUrl = null;

        if (request.HasUpdatedAvatar)
        {
            var uploadFileResult = await UploadUserAvatarAsync(request, ct);

            if (!uploadFileResult)
            {
                return G36Response.FILE_SERVICE_ERROR;
            }

            // Set the new avatar url with the new storage url.
            newAvatarUrl = request.AvatarFileInfo.StorageUrl;
        }

        var userProfile = new UserProfile
        {
            UserId = request.UserId,
            NickName = request.NickName,
            AboutMe = request.AboutMe,
            AvatarUrl = newAvatarUrl,
            UpdatedAt = DateTime.UtcNow,
        };

        var updateResult = await _g36Repository.UpdateProfileAsync(userProfile, ct);

        if (updateResult)
        {
            return G36Response.SUCCESS(newAvatarUrl);
        }

        return G36Response.DATABASE_ERROR;
    }

    private async Task<bool> UploadUserAvatarAsync(
        G36Request request,
        CancellationToken cancellationToken)
    {
        var fileService = _fileService.Value;

        var currentUserAvatarFolder = request.UserId.ToString();

        var avatarRelativeFilePath = DirectoryPathHelper.BuildPath(
            DirectoryPathHelper.WebPathSeparator,
            ROOT_USER_AVATAR_FOLDER,
            currentUserAvatarFolder);

        // Set the path for the file info to upload properly.
        request.AvatarFileInfo.FolderPath = avatarRelativeFilePath;
        request.AvatarFileInfo.FileId = request.UserId.ToString();

        var uploadResult = await fileService.UploadFileAsync(
            request.AvatarFileInfo,
            true,
            cancellationToken);

        return uploadResult.IsSuccess;
    }
}
