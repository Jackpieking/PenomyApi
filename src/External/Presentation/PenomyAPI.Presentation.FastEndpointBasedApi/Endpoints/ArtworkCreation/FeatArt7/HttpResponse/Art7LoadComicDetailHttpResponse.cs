using PenomyAPI.App.FeatArt7.OtherHandlers.LoadComicDetail;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt7.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt7.HttpResponse;

public class Art7LoadComicDetailHttpResponse : AppHttpResponse<Art7LoadComicDetailResponseDto>
{
    public static string GetAppCode(Art7LoadComicDetailResponseStatusCode responseStatusCode)
    {
        return $"Art7.LoadComicDetail.{responseStatusCode}.{(int) responseStatusCode}";
    }
}
