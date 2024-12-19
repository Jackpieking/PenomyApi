using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt17;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt17.HttpResponses;

public class Art17HttpResponse : AppHttpResponse<EmptyDto>
{
    public static string GetAppCode(Art17ResponseAppCode appCode)
    {
        return $"Art17.EDIT_ANIME.{appCode}.{(int) appCode}";
    }

    public static readonly Art17HttpResponse SUCCESS = new()
    {
        HttpCode = StatusCodes.Status200OK,
        AppCode = GetAppCode(Art17ResponseAppCode.SUCCESS),
    };

    public static readonly Art17HttpResponse INVALID_FILE_EXTENSION = new()
    {
        HttpCode = StatusCodes.Status400BadRequest,
        AppCode = GetAppCode(Art17ResponseAppCode.INVALID_FILE_EXTENSION),
    };

    public static readonly Art17HttpResponse INVALID_JSON_SCHEMA_FROM_INPUT_CATEGORIES = new()
    {
        HttpCode = StatusCodes.Status400BadRequest,
        AppCode = GetAppCode(Art17ResponseAppCode.INVALID_JSON_SCHEMA_FROM_INPUT_CATEGORIES),
    };

    public static readonly Art17HttpResponse INVALID_FILE_FORMAT = new()
    {
        HttpCode = StatusCodes.Status400BadRequest,
        AppCode = GetAppCode(Art17ResponseAppCode.INVALID_FILE_FORMAT),
    };

    public static readonly Art17HttpResponse CURRENT_CREATOR_IS_NOT_AUTHORIZED = new()
    {
        HttpCode = StatusCodes.Status401Unauthorized,
        AppCode = GetAppCode(Art17ResponseAppCode.CURRENT_CREATOR_IS_NOT_AUTHORIZED),
    };

    public static readonly Art17HttpResponse FILE_SERVICE_ERROR = new()
    {
        HttpCode = StatusCodes.Status500InternalServerError,
        AppCode = GetAppCode(Art17ResponseAppCode.FILE_SERVICE_ERROR),
    };

    public static readonly Art17HttpResponse DATABASE_ERROR = new()
    {
        HttpCode = StatusCodes.Status500InternalServerError,
        AppCode = GetAppCode(Art17ResponseAppCode.DATABASE_ERROR),
    };

    public static readonly Art17HttpResponse FILE_SIZE_IS_EXCEED_THE_LIMIT = new()
    {
        HttpCode = StatusCodes.Status400BadRequest,
        AppCode = GetAppCode(Art17ResponseAppCode.FILE_SIZE_IS_EXCEED_THE_LIMIT),
    };

    public static Art17HttpResponse MapFrom(Art17Response response)
    {
        if (response.AppCode == Art17ResponseAppCode.SUCCESS)
        {
            return SUCCESS;
        }

        if (response.AppCode == Art17ResponseAppCode.CURRENT_CREATOR_IS_NOT_AUTHORIZED)
        {
            return CURRENT_CREATOR_IS_NOT_AUTHORIZED;
        }

        if (response.AppCode == Art17ResponseAppCode.DATABASE_ERROR)
        {
            return DATABASE_ERROR;
        }

        return FILE_SERVICE_ERROR;
    }
}
