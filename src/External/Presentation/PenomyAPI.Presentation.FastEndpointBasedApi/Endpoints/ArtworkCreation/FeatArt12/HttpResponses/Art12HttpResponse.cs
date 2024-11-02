using PenomyAPI.App.FeatArt12;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt12.HttpResponses;

public sealed class Art12HttpResponse : AppHttpResponse<object>
{
    public static string GetAppCode(Art12ResponseAppCode appCode)
    {
        return $"Art12.{appCode}.{(int) appCode}";
    }
}
