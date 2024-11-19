using System.Collections.Generic;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G21.DTOs;

public class G21ResponseDto
{
    public List<G21ResponseDtoObject> CommentList { get; set; }
}

public class G21ResponseDtoObject
{
    public string Id { get; set; }
    public string Content { get; set; }
    public long LikeCount { get; set; }
    public bool IsAuthor { get; set; }
    public string Username { get; set; }
    public string Avatar { get; set; }
    public string PostDate { get; set; }
    public int TotalReplies { get; set; }
    public string CreatedBy { get; set; }
    public bool IsLiked { get; set; }
}
