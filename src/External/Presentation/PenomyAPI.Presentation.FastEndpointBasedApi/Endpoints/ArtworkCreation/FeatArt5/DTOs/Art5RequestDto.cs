using PenomyAPI.App.FeatArt5;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt5.DTOs;

public sealed class Art5RequestDto
{
    public long ComicId { get; set; }

    public Art5Request MapToRequest(long creatorId)
    {
        return new()
        {
            ComicId = ComicId,
            CreatorId = creatorId,
        };
    }
}
