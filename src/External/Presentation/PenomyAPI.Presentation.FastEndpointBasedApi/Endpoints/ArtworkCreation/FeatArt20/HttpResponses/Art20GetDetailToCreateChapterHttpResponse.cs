using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt20.OtherHandlers.GetDetailToCreateChapter;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt20.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt20.HttpResponses;

public class Art20GetDetailToCreateChapterHttpResponse : AppHttpResponse<AnimeDetailToCreateChapterResponseDto>
{
    public static string GetAppCode(Art20GetDetailToCreateChapResponseAppCode appCode)
    {
        return $"Art20.{appCode}.{(int) appCode}";
    }

    public static readonly Art20GetDetailToCreateChapterHttpResponse ARTWORK_IS_NOT_AUTHORIZED_FOR_CURRENT_CREATOR = new()
    {
        HttpCode = StatusCodes.Status401Unauthorized,
        AppCode = GetAppCode(Art20GetDetailToCreateChapResponseAppCode.ARTWORK_IS_NOT_AUTHORIZED_FOR_CURRENT_CREATOR),
    };

    public static Art20GetDetailToCreateChapterHttpResponse SUCCESS(Art20GetDetailToCreateChapResponse response)
    {
        return new()
        {
            HttpCode = StatusCodes.Status200OK,
            AppCode = GetAppCode(Art20GetDetailToCreateChapResponseAppCode.SUCCESS),
            Body = new()
            {
                Title = response.ArtworkDetail.Title,
                LastChapterUploadOrder = response.ArtworkDetail.LastChapterUploadOrder
            }
        };
    }

    public static Art20GetDetailToCreateChapterHttpResponse MapFrom(Art20GetDetailToCreateChapResponse response)
    {
        if (response.AppCode == Art20GetDetailToCreateChapResponseAppCode.SUCCESS)
        {
            return SUCCESS(response);
        }

        return ARTWORK_IS_NOT_AUTHORIZED_FOR_CURRENT_CREATOR;
    }
}
