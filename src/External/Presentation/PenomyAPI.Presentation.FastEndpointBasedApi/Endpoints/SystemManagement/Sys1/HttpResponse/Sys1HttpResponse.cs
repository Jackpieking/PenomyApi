using PenomyAPI.App.Sys1;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Sys1.HttpResponse;

public class Sys1HttpResponse : AppHttpResponse<Sys1Response>
{
    public static string GetAppCode(Sys1ResponseStatusCode Sys1ResponseStatusCode)
    {
        return $"Sys1.{Sys1ResponseStatusCode}.{(int)Sys1ResponseStatusCode}";
    }
}
