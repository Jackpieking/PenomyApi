using PenomyAPI.App.FeatArt12.OtherHandlers.ReloadChapterMedias;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt12.DTOs.GetChapterDetail;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;
using System.Collections.Generic;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt12.HttpResponses;

public class Art12ReloadChapterMediaHttpResponse
    : AppHttpResponse<IEnumerable<Art12ChapterMediaItemResponseDto>>
{
    public static string GetAppCode(Art12ReloadChapterMediaResponseAppCode appCode)
    {
        return $"Art12.{appCode}.{(int) appCode}";
    }
}
