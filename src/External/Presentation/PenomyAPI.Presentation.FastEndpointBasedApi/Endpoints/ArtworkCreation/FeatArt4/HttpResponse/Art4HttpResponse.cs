using PenomyAPI.App.FeatArt4;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt4.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt4.HttpResponse
{
    public class Art4HttpResponse : AppHttpResponse<Art4ResponseDto>
    {
        public static string GetAppCode(Art4ResponseStatusCode art4ResponseStatusCode)
        {
            return $"Art4.{art4ResponseStatusCode}.{(int)art4ResponseStatusCode}";
        }
    }
}
