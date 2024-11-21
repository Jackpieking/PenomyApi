namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG35.DTOs;

public sealed class G35RequestDto
{
    public string AccessToken { get; set; }

    public long UserId { get; set; }

    public bool IsUserIdMatched(long userId)
    {
        return UserId.Equals(userId);
    }
}
