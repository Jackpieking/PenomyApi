using System.Collections.Generic;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM23.DTOs;

public class SM23ResponseDto
{
    public List<SM23ResponseObjectDto> Comments { get; set; }
}

public class SM23ResponseObjectDto
{
    public string Id { get; set; }
    public string Content { get; set; }
    public long LikeCount { get; set; }
    public bool IsCommentAuthor { get; set; }
    public string Username { get; set; }
    public string Avatar { get; set; }
    public string PostDate { get; set; }
    public int TotalReplies { get; set; }
    public bool IsLiked { get; set; }
}
