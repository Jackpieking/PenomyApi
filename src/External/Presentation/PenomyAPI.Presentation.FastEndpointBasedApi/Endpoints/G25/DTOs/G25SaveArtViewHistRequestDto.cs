using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25.DTOs;

public class G25SaveArtViewHistRequestDto
{
    public long ArtworkId { get; set; }

    public long ChapterId { get; set; }

    public ArtworkType ArtworkType { get; set; }
}
