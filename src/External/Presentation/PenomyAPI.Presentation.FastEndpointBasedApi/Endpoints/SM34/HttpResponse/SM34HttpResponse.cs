using PenomyAPI.App.SM34;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM34.DTOs;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM34.HttpResponse;

public class SM34HttpResponse : AppHttpResponse<SM34ResponseDto>
{
    public static string GetAppCode(SM34ResponseStatusCode sm34ResponseStatusCode)
    {
        return $"SM34.{sm34ResponseStatusCode}.{(int)sm34ResponseStatusCode}";
    }
}
