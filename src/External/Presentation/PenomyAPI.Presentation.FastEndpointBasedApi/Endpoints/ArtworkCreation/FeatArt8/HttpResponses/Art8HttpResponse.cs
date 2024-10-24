using PenomyAPI.App.FeatArt8;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt8.HttpResponses;

public sealed class Art8HttpResponse : AppHttpResponse<object>
{
    public static string GetAppCode(Art8ResponseAppCode appCode)
    {
        return $"Art8.{appCode}.{(int) appCode}";
    }
}
