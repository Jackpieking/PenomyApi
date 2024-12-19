using PenomyAPI.App.FeatG19;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG19.DTOs;

public class G19RequestDto
{
    public long AnimeId { get; set; }

    public long ChapterId { get; set; }

    public G19Request MapToRequest(long userId)
    {
        return new()
        {
            UserId = userId,
            AnimeId = AnimeId,
            ChapterId = ChapterId,
        };
    }
}
