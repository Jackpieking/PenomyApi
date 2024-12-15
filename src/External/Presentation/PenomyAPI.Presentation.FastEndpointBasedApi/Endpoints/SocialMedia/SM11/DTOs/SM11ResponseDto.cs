using System;
using System.Collections.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.SM11.DTOs;

public class SM11ResponseDto
{
    public List<GroupPostDto> GroupPosts { get; set; }
}

public class GroupPostDto
{
    public string Id { get; set; }
    public string UserNickName { get; set; }
    public string UserAvatar { get; set; }
    public string GroupId { get; set; }
    public string GroupName { get; set; }
    public string GroupAvatar { get; set; }
    public string Content { get; set; }
    public string CreatedBy { get; set; }
    public string CreatedAt { get; set; }
    public bool AllowComment { get; set; }
    public long TotalLikes { get; set; }
    public List<AttachMediaDto> AttachedMedias { get; set; }
    public bool HasLikedPost { get; set; }
}

public class AttachMediaDto
{
    public string FileName { get; set; }
    public GroupPostAttachedMediaType MediaType { get; set; }
    public string StorageUrl { get; set; }
    public int UploadOrder { get; set; }
}
