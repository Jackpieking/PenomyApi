using PenomyAPI.App.FeatArt8;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt8.DTOs;

public sealed class Art8RequestDto
{
    public long ArtworkId { get; set; }

    public Art8Request MapToRequest(long removedBy)
    {
        return new()
        {
            ArtworkId = ArtworkId,
            RemovedBy = removedBy
        };
    }
}
