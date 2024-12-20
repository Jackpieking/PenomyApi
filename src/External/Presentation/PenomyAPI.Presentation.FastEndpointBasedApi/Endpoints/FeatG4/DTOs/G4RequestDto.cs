using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG4.DTOs
{
    public class G4RequestDto
    {
        public string AccessToken { get; set; }

        public long GuestId { get; set; }

        public ArtworkType ArtworkType { get; set; } = ArtworkType.Comic;
    }
}
