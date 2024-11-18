using PenomyAPI.App.SM12;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM12.DTOs;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM12.HttpResponse;

public class SM12HttpResponse : AppHttpResponse<SM12ResponseDto>
{
    public static string GetAppCode(SM12ResponseStatusCode sm12ResponseStatusCode)
    {
        return $"SM12.{sm12ResponseStatusCode}.{(int)sm12ResponseStatusCode}";
    }
}
