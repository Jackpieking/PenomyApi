using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG28.DTOs;

public class G28RequestDto
{
    public long CreatorId { get; set; }

    public ArtworkType ArtworkType { get; set; }

    public int PageNumber { get; set; }
}
