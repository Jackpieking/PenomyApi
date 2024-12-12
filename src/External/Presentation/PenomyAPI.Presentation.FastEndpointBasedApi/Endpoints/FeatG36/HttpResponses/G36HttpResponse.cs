using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG36;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG36.HttpResponses;

public class G36HttpResponse : AppHttpResponse<G36Response>
{
    public static G36HttpResponse SUCCESS(G36Response response) => new()
    {
        AppCode = GetAppCode(G36ResponseAppCode.SUCCESS),
        HttpCode = StatusCodes.Status200OK,
        Body = response,
    };

    public static readonly G36HttpResponse NICKNAME_IS_ALREADY_EXISTED = new()
    {
        AppCode = GetAppCode(G36ResponseAppCode.NICKNAME_IS_ALREADY_EXISTED),
        HttpCode = StatusCodes.Status400BadRequest,
    };

    public static readonly G36HttpResponse INVALID_FILE_UPLOAD = new()
    {
        AppCode = GetAppCode(G36ResponseAppCode.INVALID_FILE_UPLOAD),
        HttpCode = StatusCodes.Status400BadRequest,
    };

    public static readonly G36HttpResponse FILE_SERVICE_ERROR = new()
    {
        AppCode = GetAppCode(G36ResponseAppCode.FILE_SERVICE_ERROR),
        HttpCode = StatusCodes.Status500InternalServerError,
    };

    public static readonly G36HttpResponse DATABASE_ERROR = new()
    {
        AppCode = GetAppCode(G36ResponseAppCode.DATABASE_ERROR),
        HttpCode = StatusCodes.Status500InternalServerError,
    };

    public static G36HttpResponse MapFrom(G36Response response)
    {
        if (response.AppCode == G36ResponseAppCode.SUCCESS)
        {
            return SUCCESS(response);
        }

        if (response.AppCode == G36ResponseAppCode.NICKNAME_IS_ALREADY_EXISTED)
        {
            return FILE_SERVICE_ERROR;
        }

        if (response.AppCode == G36ResponseAppCode.FILE_SERVICE_ERROR)
        {
            return FILE_SERVICE_ERROR;
        }

        return DATABASE_ERROR;
    }

    private static string GetAppCode(G36ResponseAppCode appCode)
    {
        return $"G36.UPDATE_PROFILE.{appCode}.{(int) appCode}";
    }
}
