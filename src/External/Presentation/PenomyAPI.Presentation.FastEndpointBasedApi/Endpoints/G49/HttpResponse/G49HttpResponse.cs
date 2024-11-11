using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G49.DTOs;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G49.HttpResponse;

public class G49HttpResponse : AppHttpResponse<G49ResponseDto>
{
    public G49HttpResponse()
    {
    }

    public G49HttpResponse(string appCode)
    {
        AppCode = appCode;
    }
}
