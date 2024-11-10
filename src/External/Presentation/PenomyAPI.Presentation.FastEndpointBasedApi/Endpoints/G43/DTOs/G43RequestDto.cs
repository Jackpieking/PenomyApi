using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G43.DTOs
{
    public class G43RequestDto
    {
        public long ArtworkId { get; set; }
        public ArtworkType ArtworkType { get; set; }
    }
}
