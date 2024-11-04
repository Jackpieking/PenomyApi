using FastEndpoints;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G62.DTOs;

public class G62RequestDto
{
    [FromBody]
    public long CreatorId { get; set; }
}
