using Microsoft.AspNetCore.Http;
using PenomyAPI.App.G25.OtherHandlers.AddGuestViewHistory;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25.HttpResponse;

public class G25AddGuestViewHistoryHttpResponse : AppHttpResponse<object>
{
    public static readonly G25AddGuestViewHistoryHttpResponse SUCCESS = new()
    {
        AppCode = G25AddGuestViewHistoryResponseAppCode.SUCCESS.ToString(),
        HttpCode = StatusCodes.Status200OK,
    };

    public static readonly G25AddGuestViewHistoryHttpResponse GUEST_ID_NOT_FOUND = new()
    {
        AppCode = G25AddGuestViewHistoryResponseAppCode.GUEST_ID_NOT_FOUND.ToString(),
        HttpCode = StatusCodes.Status404NotFound,
    };

    public static readonly G25AddGuestViewHistoryHttpResponse CHAPTER_IS_NOT_FOUND = new()
    {
        AppCode = G25AddGuestViewHistoryResponseAppCode.CHAPTER_IS_NOT_FOUND.ToString(),
        HttpCode = StatusCodes.Status404NotFound,
    };

    public static readonly G25AddGuestViewHistoryHttpResponse DATABASE_ERROR = new()
    {
        AppCode = G25AddGuestViewHistoryResponseAppCode.DATABASE_ERROR.ToString(),
        HttpCode = StatusCodes.Status500InternalServerError,
    };

    public static G25AddGuestViewHistoryHttpResponse MapFrom(G25AddGuestViewHistoryResponse response)
    {
        if (response.AppCode == G25AddGuestViewHistoryResponseAppCode.SUCCESS)
        {
            return SUCCESS;
        }

        if (response.AppCode == G25AddGuestViewHistoryResponseAppCode.GUEST_ID_NOT_FOUND)
        {
            return GUEST_ID_NOT_FOUND;
        }

        if (response.AppCode == G25AddGuestViewHistoryResponseAppCode.CHAPTER_IS_NOT_FOUND)
        {
            return CHAPTER_IS_NOT_FOUND;
        }

        return DATABASE_ERROR;
    }
}
