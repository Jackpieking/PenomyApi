using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25.DTOs;

public class G25RequestDto
{
    public ArtworkType ArtworkType { get; set; }

    public int PageNum { get; set; }
}
