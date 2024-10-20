using PenomyAPI.App.FeatArt10;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt10.HttpResponses;

public sealed class Art10HttpResponse : AppHttpResponse<object>
{
    public static string GetAppCode(Art10ResponseAppCode appCode)
    {
        return $"Art10.{appCode}.{(int) appCode}";
    }
}
