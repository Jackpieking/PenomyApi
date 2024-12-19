using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt15;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt15.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt15.HttpResponses;

public class Art15HttpResponse : AppHttpResponse<Art15ResponseDto>
{
    public static string GetAppCode(Art15ResponseAppCode appCode)
    {
        return $"Art15.{appCode}.{(int) appCode}";
    }

    public static Art15HttpResponse MapFrom(Art15Response response)
    {
        if (response.AppCode == Art15ResponseAppCode.SUCCESS)
        {
            return SUCCESS(response.ArtworkId);
        }

        if (response.AppCode == Art15ResponseAppCode.DATABASE_ERROR)
        {
            return FILE_SERVICE_ERROR;
        }

        return DATABASE_ERROR;
    }

    public static Art15HttpResponse SUCCESS(long artworkId) => new()
    {
        AppCode = GetAppCode(Art15ResponseAppCode.SUCCESS),
        HttpCode = StatusCodes.Status200OK,
        Body = new()
        {
            ArtworkId = artworkId.ToString()
        }
    };

    public static readonly Art15HttpResponse DATABASE_ERROR = new()
    {
        AppCode = GetAppCode(Art15ResponseAppCode.DATABASE_ERROR),
        HttpCode = StatusCodes.Status500InternalServerError,
    };

    public static readonly Art15HttpResponse FILE_SERVICE_ERROR = new()
    {
        AppCode = GetAppCode(Art15ResponseAppCode.FILE_SERVICE_ERROR),
        HttpCode = StatusCodes.Status500InternalServerError,
    };

    public static readonly Art15HttpResponse INVALID_FILE_FORMAT = new()
    {
        AppCode = GetAppCode(Art15ResponseAppCode.INVALID_FILE_FORMAT),
        HttpCode = StatusCodes.Status400BadRequest,
    };

    public static readonly Art15HttpResponse INVALID_FILE_EXTENSION = new()
    {
        AppCode = GetAppCode(Art15ResponseAppCode.INVALID_FILE_EXTENSION),
        HttpCode = StatusCodes.Status400BadRequest,
    };

    public static readonly Art15HttpResponse FILE_SIZE_IS_EXCEED_THE_LIMIT = new()
    {
        AppCode = GetAppCode(Art15ResponseAppCode.FILE_SIZE_IS_EXCEED_THE_LIMIT),
        HttpCode = StatusCodes.Status400BadRequest,
    };

    public static readonly Art15HttpResponse INVALID_JSON_SCHEMA_FROM_INPUT_CATEGORIES = new()
    {
        AppCode = GetAppCode(Art15ResponseAppCode.INVALID_JSON_SCHEMA_FROM_INPUT_CATEGORIES),
        HttpCode = StatusCodes.Status400BadRequest,
    };
}
