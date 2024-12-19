using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt20;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt20.HttpResponses;

public class Art20HttpResponse : AppHttpResponse<object>
{
    public static string GetAppCode(Art20ResponseAppCode appCode)
    {
        return $"Art20.{appCode}.{(int) appCode}";
    }

    public static readonly Art20HttpResponse SUCCESS = new()
    {
        HttpCode = StatusCodes.Status200OK,
        AppCode = GetAppCode(Art20ResponseAppCode.SUCCESS),
    };

    public static readonly Art20HttpResponse INVALID_FILE_EXTENSION = new()
    {
        HttpCode = StatusCodes.Status400BadRequest,
        AppCode = GetAppCode(Art20ResponseAppCode.INVALID_FILE_EXTENSION),
    };

    public static readonly Art20HttpResponse INVALID_FILE_FORMAT = new()
    {
        HttpCode = StatusCodes.Status400BadRequest,
        AppCode = GetAppCode(Art20ResponseAppCode.INVALID_FILE_FORMAT),
    };

    public static readonly Art20HttpResponse FILE_SIZE_IS_EXCEED_THE_LIMIT = new()
    {
        HttpCode = StatusCodes.Status400BadRequest,
        AppCode = GetAppCode(Art20ResponseAppCode.FILE_SIZE_IS_EXCEED_THE_LIMIT),
    };

    public static readonly Art20HttpResponse FILE_SERVICE_ERROR = new()
    {
        HttpCode = StatusCodes.Status500InternalServerError,
        AppCode = GetAppCode(Art20ResponseAppCode.FILE_SERVICE_ERROR),
    };

    public static readonly Art20HttpResponse DATABASE_ERROR = new()
    {
        HttpCode = StatusCodes.Status500InternalServerError,
        AppCode = GetAppCode(Art20ResponseAppCode.DATABASE_ERROR),
    };

    public static Art20HttpResponse MapFrom(Art20Response response)
    {
        if (response.AppCode == Art20ResponseAppCode.SUCCESS)
        {
            return SUCCESS;
        }

        if (response.AppCode == Art20ResponseAppCode.DATABASE_ERROR)
        {
            return DATABASE_ERROR;
        }

        return FILE_SERVICE_ERROR;
    }
}