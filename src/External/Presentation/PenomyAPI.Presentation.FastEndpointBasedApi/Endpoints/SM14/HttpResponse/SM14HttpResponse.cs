using PenomyAPI.App.SM14;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM14.DTOs;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM14.HttpResponse;

public class SM14HttpResponse : AppHttpResponse<SM14ResponseDto>
{
    public static string GetAppCode(SM14ResponseStatusCode appCode)
    {
        return $"SM14.{appCode}.{(int)appCode}";
    }
}
