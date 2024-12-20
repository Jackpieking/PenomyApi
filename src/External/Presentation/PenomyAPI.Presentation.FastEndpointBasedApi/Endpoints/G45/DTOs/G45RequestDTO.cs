using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G45.DTOs;

public class G45RequestDTO
{
    public ArtworkType ArtworkType { get; set; }
    public int PageNum { get; set; }
}
