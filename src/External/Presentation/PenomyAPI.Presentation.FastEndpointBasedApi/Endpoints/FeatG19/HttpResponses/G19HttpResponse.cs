using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG19;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG19.DTOs;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG19.HttpResponses;

public class G19HttpResponse : AppHttpResponse<G19ResponseDto>
{
    public static string GetAppCode(G19ResponseAppCode appCode)
    {
        return $"G19.ANIME_CHAPTER_DETAIL.{appCode}.{(int) appCode}";
    }

    public static G19HttpResponse SUCCESS(G19Response response)
    {
        return new()
        {
            HttpCode = StatusCodes.Status200OK,
            AppCode = GetAppCode(G19ResponseAppCode.SUCCESS),
            Body = G19ResponseDto.MapFrom(response),
        };
    }

    public static readonly G19HttpResponse CHAPTER_IS_NOT_FOUND = new()
    {
        HttpCode = StatusCodes.Status404NotFound,
        AppCode = GetAppCode(G19ResponseAppCode.CHAPTER_IS_NOT_FOUND)
    };

    public static G19HttpResponse MapFrom(G19Response response)
    {
        if (response.AppCode == G19ResponseAppCode.SUCCESS)
        {
            return SUCCESS(response);
        }

        return CHAPTER_IS_NOT_FOUND;
    }
}
