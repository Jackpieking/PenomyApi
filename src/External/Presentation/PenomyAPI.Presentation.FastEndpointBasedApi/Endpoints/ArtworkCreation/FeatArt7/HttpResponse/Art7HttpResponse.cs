using PenomyAPI.App.FeatArt7;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt7.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt7.HttpResponse;

public sealed class Art7HttpResponse : AppHttpResponse<Art7ResponseDto>
{
    public static string GetAppCode(Art7ResponseStatusCode responseStatusCode)
    {
        return $"Art7.{responseStatusCode}.{(int) responseStatusCode}";
    }
}