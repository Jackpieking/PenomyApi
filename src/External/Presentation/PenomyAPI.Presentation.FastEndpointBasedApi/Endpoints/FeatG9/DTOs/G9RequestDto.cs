using PenomyAPI.App.FeatG9;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG9.DTOs;

public sealed class G9RequestDto
{
    public long ComicId { get; set; }

    public long ChapterId { get; set; }

    public G9Request MapToRequest(long userId)
    {
        return new()
        {
            UserId = userId,
            ComicId = ComicId,
            ChapterId = ChapterId,
        };
    }
}
