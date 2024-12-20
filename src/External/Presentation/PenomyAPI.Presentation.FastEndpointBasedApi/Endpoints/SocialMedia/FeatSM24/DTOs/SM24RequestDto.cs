namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM24.DTOs;

public class SM24RequestDto
{
    public string CommentContent { get; init; }

    public bool IsGroupPostComment { get; init; }

    public string PostId { get; set; }
}
