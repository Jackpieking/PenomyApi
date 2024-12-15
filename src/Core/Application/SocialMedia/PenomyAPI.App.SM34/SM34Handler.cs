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

namespace PenomyAPI.App.SM34;

public class SM34Handler : IFeatureHandler<SM34Request, SM34Response>
{
    private readonly Lazy<IDefaultDistributedFileService> _fileService;
    private readonly ISM34Repository _sm34Repository;

    public SM34Handler(
        Lazy<IUnitOfWork> unitOfWork,
        Lazy<IDefaultDistributedFileService> fileService
    )
    {
        _sm34Repository = unitOfWork.Value.FeatSM34Repository;
        _fileService = fileService;
    }

    public async Task<SM34Response> ExecuteAsync(SM34Request request, CancellationToken ct)
    {
        var userProfile = await _sm34Repository.GetUserProfileAsync(request.UserId, ct);
        if (userProfile == null)
            return new SM34Response
            {
                IsSuccess = false,
                ErrorMessages = ["User profile not found"],
                StatusCode = SM34ResponseStatusCode.USER_PROFILE_NOT_FOUND,
            };
        var artworkFolderName = request.GroupPostId.ToString();

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
            return new SM34Response
            {
                IsSuccess = false,
                ErrorMessages = ["Cannot create folder using file service"],
                StatusCode = SM34ResponseStatusCode.FILE_SERVICE_ERROR,
            };
        List<GroupPostAttachedMedia> mediaList = [];
        if (request.AppFileInfos.Any())
            foreach (var fileInfo in request.AppFileInfos)
            {
                //fileInfo.FileId =
                fileInfo.FolderPath = folderInfo.RelativePath;
                var uploadResult = await fileService.UploadFileAsync(fileInfo, false, ct);
                var groupPostMedia = new GroupPostAttachedMedia
                {
                    StorageUrl = uploadResult.Value.StorageUrl,
                    PostId = request.GroupPostId,
                    MediaType = ConvertToGroupPostAttachedMediaType(fileInfo.FileExtension),
                    FileName = fileInfo.FileName,
                    Id = long.Parse(fileInfo.FileId),
                    UploadOrder = mediaList.Count + 1,
                };
                mediaList.Add(groupPostMedia);
                if (!uploadResult.IsSuccess)
                    return new SM34Response
                    {
                        IsSuccess = false,
                        ErrorMessages = ["Cannot create folder using file service"],
                        StatusCode = SM34ResponseStatusCode.FILE_SERVICE_ERROR,
                    };
            }

        var dateTimeNow = DateTime.UtcNow;
        var groupPost = new GroupPost
        {
            Id = request.GroupPostId,
            Content = request.Content,
            GroupId = request.GroupId,
            TotalLikes = 0,
            AllowComment = request.AllowComment,
            PostStatus = GroupPostStatus.Approved,
            ApprovedBy = request.UserId,
            ApprovedAt = dateTimeNow,
            CreatedBy = request.UserId,
            CreatedAt = dateTimeNow,
            UpdatedAt = dateTimeNow,
        };
        var groupPostStatistic = GroupPostLikeStatistic.Empty(request.GroupPostId);

        var result = await _sm34Repository.CreateGroupPostAsync(
            groupPost,
            mediaList,
            groupPostStatistic,
            ct
        );
        if (!result)
            return new SM34Response
            {
                IsSuccess = false,
                StatusCode = SM34ResponseStatusCode.DATABASE_ERROR,
            };

        return new SM34Response
        {
            IsSuccess = true,
            UserPostId = groupPost.Id,
            StatusCode = SM34ResponseStatusCode.SUCCESS,
        };
    }

    private GroupPostAttachedMediaType ConvertToGroupPostAttachedMediaType(string extension)
    {
        return extension.ToLower() switch
        {
            "jpg" or "jpeg" or "png" or "gif" or "bmp" => GroupPostAttachedMediaType.Image,
            "mp4" or "mkv" => GroupPostAttachedMediaType.Video,
            _ => throw new NotSupportedException($"Unsupported file extension: {extension}"),
        };
    }
}
