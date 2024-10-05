using PenomyAPI.App.FeatArt4;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatArt4.DTOs;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG3.HttpResponse
{
    public class Art4HttpResponse : AppHttpResponse<Art4ResponseDto>
    {
        public static string GetAppCode(Art4ResponseStatusCode art4ResponseStatusCode)
        {
            return $"Art4.{art4ResponseStatusCode}.{(int) art4ResponseStatusCode}";
        }
    }
}
