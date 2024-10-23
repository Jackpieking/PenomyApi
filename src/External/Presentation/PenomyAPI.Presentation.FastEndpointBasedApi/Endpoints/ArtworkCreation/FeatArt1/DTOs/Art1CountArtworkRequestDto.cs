using PenomyAPI.App.FeatArt1.OtherHandlers.CountArtwork;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt1.DTOs;

public class Art1CountArtworkRequestDto
{
    public ArtworkType ArtworkType { get; set; }

    public Art1CountArtworkRequest MapToRequest(long creatorId)
    {
        return new()
        {
            CreatorId = creatorId,
            ArtworkType = ArtworkType,
        };
    }
}
