using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt22.OtherHandlers.GetChapterDetail;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt22.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt22.HttpResponses;

public class Art22GetChapterDetailHttpResponse : AppHttpResponse<Art22GetChapterDetailResponseDto>
{
    public static string GetAppCode(Art22GetChapterDetailResponseAppCode appCode)
    {
        return $"Art22.GET_CHAPTER_DETAIL.{appCode}.{(int) appCode}";
    }

    public static Art22GetChapterDetailHttpResponse SUCCESS(Art22GetChapterDetailResponse response)
    {
        return new()
        {
            HttpCode = StatusCodes.Status200OK,
            AppCode = GetAppCode(Art22GetChapterDetailResponseAppCode.SUCCESS),
            Body = Art22GetChapterDetailResponseDto.MapFrom(response)
        };
    }

    public static readonly Art22GetChapterDetailHttpResponse CHAPTER_IS_TEMPORARILY_REMOVED = new()
    {
        HttpCode = StatusCodes.Status404NotFound,
        AppCode = GetAppCode(Art22GetChapterDetailResponseAppCode.CHAPTER_IS_TEMPORARILY_REMOVED)
    };

    public static readonly Art22GetChapterDetailHttpResponse NO_PERMISSION_GRANTED_FOR_CURRENT_CREATOR = new()
    {
        HttpCode = StatusCodes.Status401Unauthorized,
        AppCode = GetAppCode(Art22GetChapterDetailResponseAppCode.NO_PERMISSION_GRANTED_FOR_CURRENT_CREATOR)
    };

    public static readonly Art22GetChapterDetailHttpResponse DATABASE_ERROR = new()
    {
        HttpCode = StatusCodes.Status401Unauthorized,
        AppCode = GetAppCode(Art22GetChapterDetailResponseAppCode.DATABASE_ERROR)
    };

    public static Art22GetChapterDetailHttpResponse MapFrom(Art22GetChapterDetailResponse response)
    {
        if (response.AppCode == Art22GetChapterDetailResponseAppCode.SUCCESS)
        {
            return SUCCESS(response);
        }

        if (response.AppCode == Art22GetChapterDetailResponseAppCode.DATABASE_ERROR)
        {
            return DATABASE_ERROR;
        }

        if (response.AppCode == Art22GetChapterDetailResponseAppCode.NO_PERMISSION_GRANTED_FOR_CURRENT_CREATOR)
        {
            return NO_PERMISSION_GRANTED_FOR_CURRENT_CREATOR;
        }

        return CHAPTER_IS_TEMPORARILY_REMOVED;
    }
}
