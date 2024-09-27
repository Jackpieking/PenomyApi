using Microsoft.AspNetCore.Http;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG3.DTOs
{
    public class FeatG3RequestDto
    {
        public EmptyDto param { get; set; }
    }
}
