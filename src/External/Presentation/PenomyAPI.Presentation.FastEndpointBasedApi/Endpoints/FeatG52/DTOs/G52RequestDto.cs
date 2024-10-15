namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG52.DTOs;

public class G52RequestDto
{
    public string ArtworkId { get; init; }

    public string ChapterId { get; init; }

    public string CommentContent { get; init; }

    public long ParentCommentId { get; init; }

    public bool IsDirectComment { get; init; }

    public long UserId { get; init; }
}
