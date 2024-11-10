using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G44.DTOs;

public class G44RequestDto
{
    public long ArtworkId { get; set; }
    public ArtworkType ArtworkType { get; set; }
}
