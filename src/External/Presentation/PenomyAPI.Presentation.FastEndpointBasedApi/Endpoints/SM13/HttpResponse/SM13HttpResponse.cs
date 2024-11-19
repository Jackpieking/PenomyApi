using PenomyAPI.App.SM13;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM13.DTOs;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM13.HttpResponse;

public class SM13HttpResponse : AppHttpResponse<SM13ResponseDto>
{
    public static string GetAppCode(SM13ResponseStatusCode responseStatusCode)
    {
        return $"Sm13.{responseStatusCode}.{(int)responseStatusCode}";
    }
}
