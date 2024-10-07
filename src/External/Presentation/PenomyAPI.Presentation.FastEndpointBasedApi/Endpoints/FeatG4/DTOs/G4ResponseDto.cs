using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using System.Collections.Generic;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG4.DTOs
{
    public class G4ResponseDto
    {
        public List<ArtworkCategory> ArtworkList { get; set; }
    }

}
