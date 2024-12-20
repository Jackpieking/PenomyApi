using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG9.OtherHandlers.GetChapterList;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG9.DTOs;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG9.HttpResponses;

public sealed class G9GetChapterListHttpResponse
    : AppHttpResponse<IEnumerable<G9ChapterItemResponseDto>>
{
    public static readonly G9GetChapterListHttpResponse COMIC_ID_NOT_FOUND =
        new()
        {
            AppCode =
                $"G9.GET_CHAPTER_LIST.{G9GetChapterListResponseAppCode.COMIC_ID_NOT_FOUND}.{(int)G9GetChapterListResponseAppCode.COMIC_ID_NOT_FOUND}",
            HttpCode = StatusCodes.Status404NotFound
        };

    public static G9GetChapterListHttpResponse SUCCESS(G9GetChapterListResponse response)
    {
        return new()
        {
            AppCode =
                $"G9.GET_CHAPTER_LIST.{G9GetChapterListResponseAppCode.SUCCESS}.{(int)G9GetChapterListResponseAppCode.SUCCESS}",
            HttpCode = StatusCodes.Status200OK,
            Body = response.ChapterList.Select(G9ChapterItemResponseDto.MapFrom),
        };
    }

    public static G9GetChapterListHttpResponse MapFrom(G9GetChapterListResponse response)
    {
        if (response.AppCode == G9GetChapterListResponseAppCode.SUCCESS)
        {
            return SUCCESS(response);
        }

        return COMIC_ID_NOT_FOUND;
    }
}
