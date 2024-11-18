using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.App.Common.FileServices;
using PenomyAPI.App.Common.FileServices.Models;
using PenomyAPI.App.Common.Helpers;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using PenomyAPI.Infra.Configuration.Options;

namespace PenomyAPI.App.SM13;

public class SM13Handler : IFeatureHandler<SM13Request, SM13Response>
{
    private readonly Lazy<IDefaultDistributedFileService> _fileService;
    private readonly CloudinaryOptions _options;
    private readonly ISM13Repository _sm13Repository;

    public SM13Handler(
        Lazy<IUnitOfWork> unitOfWork,
        Lazy<IDefaultDistributedFileService> fileService,
        CloudinaryOptions options
    )
    {
        _sm13Repository = unitOfWork.Value.FeatSM13Repository;
        _fileService = fileService;
        _options = options;
    }

    public async Task<SM13Response> ExecuteAsync(SM13Request request, CancellationToken ct)
    {
        IEnumerable<UserPostAttachedMedia> attachedMediae = null;
        if (!await _sm13Repository.IsUserPostExistedAsync(request.UserPostId, ct)) return SM13Response.UserPostNotFound;
        var uploadPostResult = Result<AppFileInfo>.Failed();
        if (request.IsUpdateAttachMedia)
        {
            var result = await UploadAgainPostMediaAsync(request, ct);
            uploadPostResult = result.Item1;
            if (!uploadPostResult.IsSuccess) return SM13Response.FileServiceError;
            attachedMediae = result.Item2;
        }

        var dateTimeNow = DateTime.UtcNow;
        var userPost = new UserPost
        {
            Id = request.UserPostId,
            AllowComment = request.AllowComment,
            CreatedAt = dateTimeNow,
            UpdatedAt = dateTimeNow,
            Content = request.Content,
            PublicLevel = request.PublicLevel,
            CreatedBy = request.UserId,
            TotalLikes = 0
        };
        var updateResult = await _sm13Repository.UpdateUserPostAsync(
            userPost,
            request.IsUpdateAttachMedia,
            attachedMediae,
            ct
        );

        return !updateResult
            ? SM13Response.DatabaseError
            : new SM13Response { IsSuccess = true, StatusCode = SM13ResponseStatusCode.SUCCESS };
    }

    private async Task<(Result<AppFileInfo>, List<UserPostAttachedMedia> )> UploadAgainPostMediaAsync(
        SM13Request request,
        CancellationToken ct
    )
    {
        List<UserPostAttachedMedia> mediaList = [];
        var res = Result<AppFileInfo>.Failed();
        var postFolderName = request.UserPostId.ToString();
        var folderPath = DirectoryPathHelper.BuildPath(
            DirectoryPathHelper.WebPathSeparator,
            "post",
            postFolderName
        );
        var fileService = _fileService.Value;
        foreach (var fileInfo in request.AppFileInfos)
        {
            fileInfo.FolderPath = folderPath;
            res = await fileService.UploadFileAsync(
                fileInfo,
                true,
                ct
            );
            var userPostMedia = new UserPostAttachedMedia
            {
                StorageUrl = res.Value.StorageUrl,
                PostId = request.UserPostId,
                MediaType = ConvertToUserPostAttachedMediaType(fileInfo.FileExtension),
                FileName = fileInfo.FileName,
                Id = long.Parse(fileInfo.FileId),
                UploadOrder = mediaList.Count + 1
            };
            mediaList.Add(userPostMedia);
            if (!res.IsSuccess) return (res, mediaList);
        }

        return (res, mediaList);
    }

    private UserPostAttachedMediaType ConvertToUserPostAttachedMediaType(string extension)
    {
        return extension.ToLower() switch
        {
            "jpg" or "jpeg" or "png" or "gif" or "bmp" => UserPostAttachedMediaType.Image,
            "mp4" or "mkv" => UserPostAttachedMediaType.Video,
            _ => throw new NotSupportedException($"Unsupported file extension: {extension}")
        };
    }
}
