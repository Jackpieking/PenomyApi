using System;
using System.Collections.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.SM15.DTOs;

public class SM15ResponseDto
{
    public List<UserPostDto> UserPosts { get; set; }
}

public class UserPostDto
{
    public string Id { get; set; }
    public string Content { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool AllowComment { get; set; }
    public UserPostPublicLevel PublicLevel { get; set; }
    public long TotalLikes { get; set; }
    public bool IsCurrentUserLike { get; set; }
    public List<AttachMediaDto> AttachedMedias { get; set; }
}

public class AttachMediaDto
{
    public string FileName { get; set; }
    public UserPostAttachedMediaType MediaType { get; set; }
    public string StorageUrl { get; set; }
    public int UploadOrder { get; set; }
}
