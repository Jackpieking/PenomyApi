using PenomyAPI.App.FeatArt12.OtherHandlers.GetChapterDetail;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt12.DTOs.GetChapterDetail;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt12.HttpResponses;

public sealed class Art12GetChapterDetailHttpResponse
    : AppHttpResponse<Art12GetChapterDetailResponseDto>
{
    public static string GetAppCode(Art12GetChapterDetailResponseAppCode appCode)
    {
        return $"Art12.{appCode}.{(int) appCode}";
    }
}
