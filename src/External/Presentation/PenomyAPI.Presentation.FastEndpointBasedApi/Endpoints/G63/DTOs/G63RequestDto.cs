using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G63.DTOs;

public class G63RequestDto
{
    public ArtworkType ArtworkType { get; set; }
    public int PageNum { get; set; }
}
