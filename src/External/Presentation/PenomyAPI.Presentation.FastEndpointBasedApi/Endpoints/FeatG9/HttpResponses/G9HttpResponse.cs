using PenomyAPI.App.FeatG9;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG9.DTOs;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG9.HttpResponses;

public sealed class G9HttpResponse : AppHttpResponse<G9ResponseDto>
{
    public static string GetAppCode(G9ResponseAppCode appCode)
    {
        return $"G9.{appCode}.{(int) appCode}";
    }
}
