using System.Collections.Generic;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G20.DTOs;

public class G20ResponseDto
{
    public List<G20ResponseDtoObject> CommentList { get; set; }
}
public class G20ResponseDtoObject
{
    public long Id { get; set; }
    public string Content { get; set; }
    public long LikeCount { get; set; }
    public bool IsAuthor { get; set; }
    public string Username { get; set; }
    public string Avatar { get; set; }
    public string PostDate { get; set; }
    public int TotalReplies { get; set; }
}
