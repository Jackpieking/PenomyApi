using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.App.Common.FileServices;
using PenomyAPI.App.Common.FileServices.Models;
using PenomyAPI.App.Common.Helpers;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.SM12;

public class SM12Handler : IFeatureHandler<SM12Request, SM12Response>
{
    private readonly Lazy<IDefaultDistributedFileService> _fileService;
    private readonly ISM12Repository _sm12Repository;

    public SM12Handler(
        Lazy<IUnitOfWork> unitOfWork,
        Lazy<IDefaultDistributedFileService> fileService
    )
    {
        _sm12Repository = unitOfWork.Value.FeatSM12Repository;
        _fileService = fileService;
    }

    public async Task<SM12Response> ExecuteAsync(SM12Request request, CancellationToken ct)
    {
        var userProfile = await _sm12Repository.GetUserProfileAsync(request.UserId, ct);
        if (userProfile == null)
            return new SM12Response
            {
                IsSuccess = false,
                ErrorMessages = ["User profile not found"],
                StatusCode = SM12ResponseStatusCode.USER_PROFILE_NOT_FOUND,
            };
        var artworkFolderName = request.UserPostId.ToString();

        // The info of folder to store the thumbnail of this comic.
        var folderInfo = new AppFolderInfo
        {
            RelativePath = DirectoryPathHelper.BuildPath(
                DirectoryPathHelper.WebPathSeparator,
                "posts",
                artworkFolderName
            ),
        };
        var fileService = _fileService.Value;
        var folderCreateResult = await fileService.CreateFolderAsync(folderInfo, ct);
        if (!folderCreateResult)
            return new SM12Response
            {
                IsSuccess = false,
                ErrorMessages = ["Cannot create folder using file service"],
                StatusCode = SM12ResponseStatusCode.FILE_SERVICE_ERROR,
            };
        List<UserPostAttachedMedia> mediaList = [];
        if (request.AppFileInfos.Any())
            foreach (var fileInfo in request.AppFileInfos)
            {
                //fileInfo.FileId =
                fileInfo.FolderPath = folderInfo.RelativePath;
                var uploadResult = await fileService.UploadFileAsync(fileInfo, false, ct);
                var userPostMedia = new UserPostAttachedMedia
                {
                    StorageUrl = uploadResult.Value.StorageUrl,
                    PostId = request.UserPostId,
                    MediaType = ConvertToUserPostAttachedMediaType(fileInfo.FileExtension),
                    FileName = fileInfo.FileName,
                    Id = long.Parse(fileInfo.FileId),
                    UploadOrder = mediaList.Count + 1,
                };
                mediaList.Add(userPostMedia);
                if (!uploadResult.IsSuccess)
                    return new SM12Response
                    {
                        IsSuccess = false,
                        ErrorMessages = ["Cannot create folder using file service"],
                        StatusCode = SM12ResponseStatusCode.FILE_SERVICE_ERROR,
                    };
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
            TotalLikes = 0,
        };
        var userPostStatistic = UserPostLikeStatistic.Empty(request.UserPostId);

        var result = await _sm12Repository.CreateUserPostAsync(
            userPost,
            mediaList,
            userPostStatistic,
            ct
        );
        if (!result)
            return new SM12Response
            {
                IsSuccess = false,
                StatusCode = SM12ResponseStatusCode.DATABASE_ERROR,
            };

        return new SM12Response
        {
            IsSuccess = true,
            UserPostId = userPost.Id,
            StatusCode = SM12ResponseStatusCode.SUCCESS,
        };
    }

    private UserPostAttachedMediaType ConvertToUserPostAttachedMediaType(string extension)
    {
        return extension.ToLower() switch
        {
            "jpg" or "jpeg" or "png" or "gif" or "bmp" => UserPostAttachedMediaType.Image,
            "mp4" or "mkv" => UserPostAttachedMediaType.Video,
            _ => throw new NotSupportedException($"Unsupported file extension: {extension}"),
        };
    }
}
