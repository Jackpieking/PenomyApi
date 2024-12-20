using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG47.DTOs;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG47.HttpResponse;

public class G47HttpResponse : AppHttpResponse<G47ResponseDto>
{
    public G47HttpResponse()
    {
    }

    public G47HttpResponse(string appCode)
    {
        AppCode = appCode;
    }

    public long FavoriteCount { get; set; }
}
