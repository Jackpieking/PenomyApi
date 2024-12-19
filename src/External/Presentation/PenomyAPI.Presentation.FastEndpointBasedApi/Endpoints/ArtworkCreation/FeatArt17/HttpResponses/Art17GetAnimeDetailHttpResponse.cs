using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt17.OtherHandlers.GetDetail;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt17.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt17.HttpResponses;

public class Art17GetAnimeDetailHttpResponse : AppHttpResponse<Art17AnimeDetailResponseDto>
{
    public static string GetAppCode(Art17GetDetailResponseAppCode appCode)
    {
        return $"Art17.GET_ANIME_DETAIL.{appCode}.{(int) appCode}";
    }

    public static readonly Art17GetAnimeDetailHttpResponse ID_NOT_FOUND = new()
    {
        HttpCode = StatusCodes.Status404NotFound,
        AppCode = GetAppCode(Art17GetDetailResponseAppCode.ID_NOT_FOUND),
    };

    public static readonly Art17GetAnimeDetailHttpResponse CREATOR_HAS_NO_PERMISSION = new()
    {
        HttpCode = StatusCodes.Status400BadRequest,
        AppCode = GetAppCode(Art17GetDetailResponseAppCode.CREATOR_HAS_NO_PERMISSION),
    };

    public static Art17GetAnimeDetailHttpResponse SUCCESS(Art17GetAnimeDetailResponse response) => new()
    {
        HttpCode = StatusCodes.Status200OK,
        AppCode = GetAppCode(Art17GetDetailResponseAppCode.SUCCESS),
        Body = Art17AnimeDetailResponseDto.MapFrom(response)
    };

    public static Art17GetAnimeDetailHttpResponse MapFrom(Art17GetAnimeDetailResponse response)
    {
        if (response.AppCode == Art17GetDetailResponseAppCode.SUCCESS)
        {
            return SUCCESS(response);
        }

        if (response.AppCode == Art17GetDetailResponseAppCode.CREATOR_HAS_NO_PERMISSION)
        {
            return CREATOR_HAS_NO_PERMISSION;
        }

        return ID_NOT_FOUND;
    }
}
