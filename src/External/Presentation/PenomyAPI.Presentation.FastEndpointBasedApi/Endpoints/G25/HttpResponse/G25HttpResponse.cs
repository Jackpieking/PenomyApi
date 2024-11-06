using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25.HttpResponse;

public class G25HttpResponse : AppHttpResponse<ArtworkCardDto>
{
    public ArtworkCardDto g25ResponseDtos { get; set; }
}
