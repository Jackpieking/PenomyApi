using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG19.OtherHandlers.GetChapterList;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG19.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG19.HttpResponses;

public class G19GetChapterListHttpResponse : AppHttpResponse<IEnumerable<G19ChapterItemResponseDto>>
{
    public static string GetAppCode(G19GetChapterListResponseAppCode appCode)
    {
        return $"G9.GET_CHAPTER_LIST.{appCode}.{(int) appCode}";
    }

    public static readonly G19GetChapterListHttpResponse ANIME_ID_NOT_FOUND =
        new()
        {
            AppCode = GetAppCode(G19GetChapterListResponseAppCode.ANIME_ID_NOT_FOUND),
            HttpCode = StatusCodes.Status404NotFound
        };

    public static G19GetChapterListHttpResponse SUCCESS(G19GetChapterListResponse response)
    {
        return new()
        {
            HttpCode = StatusCodes.Status200OK,
            AppCode = GetAppCode(G19GetChapterListResponseAppCode.SUCCESS),
            Body = response.ChapterList.Select(G19ChapterItemResponseDto.MapFrom),
        };
    }

    public static G19GetChapterListHttpResponse MapFrom(G19GetChapterListResponse response)
    {
        if (response.AppCode == G19GetChapterListResponseAppCode.SUCCESS)
        {
            return SUCCESS(response);
        }

        return ANIME_ID_NOT_FOUND;
    }
}
