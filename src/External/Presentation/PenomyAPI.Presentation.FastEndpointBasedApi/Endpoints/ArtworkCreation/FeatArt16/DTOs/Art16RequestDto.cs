using PenomyAPI.App.FeatArt16;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt16.DTOs;

public class Art16RequestDto
{
    public long ArtworkId { get; set; }

    public Art16Request MapToRequest(long creatorId)
    {
        return new()
        {
            ArtworkId = ArtworkId,
            CreatorId = creatorId,
        };
    }
}
