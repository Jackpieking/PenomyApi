using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt22;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt22.HttpResponses;

public class Art22UpdateAnimeChapterHttpResponse : AppHttpResponse<string>
{
    public static string GetAppCode(Art22ResponseAppCode appCode)
    {
        return $"Art22.UPDATE_ANIME_CHAPTER.{appCode}.{(int) appCode}";
    }

    public static readonly Art22UpdateAnimeChapterHttpResponse SUCCESS = new()
    {
        HttpCode = StatusCodes.Status200OK,
        AppCode = GetAppCode(Art22ResponseAppCode.SUCCESS)
    };

    public static readonly Art22UpdateAnimeChapterHttpResponse FILE_SERVICE_ERROR = new()
    {
        HttpCode = StatusCodes.Status500InternalServerError,
        AppCode = GetAppCode(Art22ResponseAppCode.FILE_SERVICE_ERROR)
    };

    public static readonly Art22UpdateAnimeChapterHttpResponse DATABASE_ERROR = new()
    {
        HttpCode = StatusCodes.Status500InternalServerError,
        AppCode = GetAppCode(Art22ResponseAppCode.DATABASE_ERROR)
    };

    public static readonly Art22UpdateAnimeChapterHttpResponse CHAPTER_IS_TEMPORARILY_REMOVED = new()
    {
        HttpCode = StatusCodes.Status404NotFound,
        AppCode = GetAppCode(Art22ResponseAppCode.CHAPTER_IS_TEMPORARILY_REMOVED)
    };

    public static readonly Art22UpdateAnimeChapterHttpResponse NO_PERMISSION_GRANTED_FOR_CURRENT_CREATOR = new()
    {
        HttpCode = StatusCodes.Status401Unauthorized,
        AppCode = GetAppCode(Art22ResponseAppCode.NO_PERMISSION_GRANTED_FOR_CURRENT_CREATOR)
    };

    public static readonly Art22UpdateAnimeChapterHttpResponse INVALID_PUBLISH_STATUS = new()
    {
        HttpCode = StatusCodes.Status400BadRequest,
        AppCode = GetAppCode(Art22ResponseAppCode.INVALID_PUBLISH_STATUS)
    };

    public static readonly Art22UpdateAnimeChapterHttpResponse INVALID_FILE_FORMAT = new()
    {
        HttpCode = StatusCodes.Status400BadRequest,
        AppCode = GetAppCode(Art22ResponseAppCode.INVALID_FILE_FORMAT)
    };

    public static readonly Art22UpdateAnimeChapterHttpResponse FILE_SIZE_IS_EXCEED_THE_LIMIT = new()
    {
        HttpCode = StatusCodes.Status400BadRequest,
        AppCode = GetAppCode(Art22ResponseAppCode.FILE_SIZE_IS_EXCEED_THE_LIMIT)
    };

    public static Art22UpdateAnimeChapterHttpResponse MapFrom(Art22Response response)
    {
        switch (response.AppCode)
        {
            case Art22ResponseAppCode.SUCCESS:
                return SUCCESS;

            case Art22ResponseAppCode.FILE_SERVICE_ERROR:
                return FILE_SERVICE_ERROR;

            case Art22ResponseAppCode.INVALID_FILE_FORMAT:
                return INVALID_PUBLISH_STATUS;

            case Art22ResponseAppCode.FILE_SIZE_IS_EXCEED_THE_LIMIT:
                return FILE_SIZE_IS_EXCEED_THE_LIMIT;

            case Art22ResponseAppCode.CHAPTER_IS_TEMPORARILY_REMOVED:
                return CHAPTER_IS_TEMPORARILY_REMOVED;

            case Art22ResponseAppCode.NO_PERMISSION_GRANTED_FOR_CURRENT_CREATOR:
                return NO_PERMISSION_GRANTED_FOR_CURRENT_CREATOR;

            case Art22ResponseAppCode.INVALID_PUBLISH_STATUS:
                return INVALID_PUBLISH_STATUS;

            case Art22ResponseAppCode.DATABASE_ERROR:
                return DATABASE_ERROR;

            default:
                return NO_PERMISSION_GRANTED_FOR_CURRENT_CREATOR;
        }
    }
}
