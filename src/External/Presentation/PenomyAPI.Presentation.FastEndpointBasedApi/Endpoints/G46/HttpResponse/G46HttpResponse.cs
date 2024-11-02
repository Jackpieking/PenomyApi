using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G46.DTOs;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G46.HttpResponse;

public class G46HttpResponse : AppHttpResponse<G46ResponseDto>
{
    public G46HttpResponse()
    {
    }

    public G46HttpResponse(string appCode)
    {
        AppCode = appCode;
    }
}
