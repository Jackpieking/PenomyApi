using PenomyAPI.App.FeatArt10.OtherHandlers.GetDetailToCreateChapter;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt10.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt10.HttpResponses;

public sealed class Art10GetDetailToCreateChapterHttpResponse
    : AppHttpResponse<ComicDetailToCreateChapterResponseDto>
{
    public static string GetAppCode(Art10GetDetailToCreateChapterResponseAppCode appCode)
    {
        return $"Art10.{appCode}.{(int) appCode}";
    }
}
