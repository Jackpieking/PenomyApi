using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt14;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt14.HttpResponses;

public sealed class Art14HttpResponse : AppHttpResponse<object>
{
    public static readonly Art14HttpResponse SUCCESS = new()
    {
        AppCode = GetAppCode(Art14ResponseAppCode.SUCCESS),
        HttpCode = StatusCodes.Status200OK,
    };

    public static readonly Art14HttpResponse CHAPTER_ID_NOT_FOUND = new()
    {
        AppCode = GetAppCode(Art14ResponseAppCode.CHAPTER_ID_NOT_FOUND),
        HttpCode = StatusCodes.Status404NotFound,
    };

    public static readonly Art14HttpResponse CREATOR_HAS_NO_PERMISSION = new()
    {
        AppCode = GetAppCode(Art14ResponseAppCode.CREATOR_HAS_NO_PERMISSION),
        HttpCode = StatusCodes.Status403Forbidden,
    };

    public static readonly Art14HttpResponse DATABASE_ERROR = new()
    {
        AppCode = GetAppCode(Art14ResponseAppCode.DATABASE_ERROR),
        HttpCode = StatusCodes.Status500InternalServerError,
    };

    public static Art14HttpResponse MapFrom(Art14Response response)
    {
        if (response.AppCode == Art14ResponseAppCode.SUCCESS)
        {
            return SUCCESS;
        }

        if (response.AppCode == Art14ResponseAppCode.CHAPTER_ID_NOT_FOUND)
        {
            return CHAPTER_ID_NOT_FOUND;
        }

        if (response.AppCode == Art14ResponseAppCode.CREATOR_HAS_NO_PERMISSION)
        {
            return CREATOR_HAS_NO_PERMISSION;
        }

        return DATABASE_ERROR;
    }

    private static string GetAppCode(Art14ResponseAppCode appCode)
    {
        return $"ART14.REMOVE_ARTWORK_CHAPTER.{appCode}.{(int) appCode}";
    }
}
