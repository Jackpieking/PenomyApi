using PenomyAPI.App.Chat3;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Chat.Chat3.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Chat.Chat3.HttpResponse;

public class Chat3HttpResponse : AppHttpResponse<Chat3ResponseDto>
{
    public static string GetAppCode(Chat3ResponseStatusCode Chat3ResponseStatusCode)
    {
        return $"Chat3.{Chat3ResponseStatusCode}.{(int)Chat3ResponseStatusCode}";
    }
}
