using PenomyAPI.App.FeatArt5;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt5.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt5.HttpResponses;

public class Art5HttpResponse : AppHttpResponse<ComicDetailResponseDto>
{
    public static string GetAppCode(Art5ResponseAppCode appCode)
    {
        return $"Art5.{appCode}.{(int) appCode}";
    }
}
