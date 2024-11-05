using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG50.DTOs;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG50.HttpResponse;

public class G50HttpResponse : AppHttpResponse<G50ResponseDto>
{
    public G50HttpResponse()
    {
    }

    public G50HttpResponse(string appCode)
    {
        AppCode = appCode;
    }
}
