using System.Collections.Generic;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G59.DTOs;

public class G59ResponseDto
{
    public List<G59ResponseDtoObject> CommentList { get; set; }
}

public class G59ResponseDtoObject
{
    public string Id { get; set; }
    public string Content { get; set; }
    public long LikeCount { get; set; }
    public bool IsArtworkAuthor { get; set; }
    public bool IsCommentAuthor { get; set; }
    public string Username { get; set; }
    public string Avatar { get; set; }
    public string PostDate { get; set; }
    public int TotalReplies { get; set; }
    public string CreatedBy { get; set; }
    public bool IsLiked { get; set; }
}
