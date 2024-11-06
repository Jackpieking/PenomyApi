using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G48.DTOs
{
    public class G48RequestDTOs
    {
        public ArtworkType ArtworkType { get; set; }
        public int PageNum { get; set; }
    }
}
