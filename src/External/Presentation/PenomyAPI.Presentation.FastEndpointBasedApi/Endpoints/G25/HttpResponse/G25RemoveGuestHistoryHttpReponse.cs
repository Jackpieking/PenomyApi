using Microsoft.AspNetCore.Http;
using PenomyAPI.App.G25.OtherHandlers.RemoveGuestHistoryItem;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25.HttpResponse;

public class G25RemoveGuestHistoryHttpReponse : AppHttpResponse<object>
{
    public static readonly G25RemoveGuestHistoryHttpReponse SUCCESS = new()
    {
        HttpCode = StatusCodes.Status200OK,
        AppCode = $"G25.REMOVE_GUEST_HISTORY.{G25RemoveGuestHistoryItemResponseAppCode.SUCCESS}.{(int) G25RemoveGuestHistoryItemResponseAppCode.SUCCESS}",
    };

    public static readonly G25RemoveGuestHistoryHttpReponse GUEST_ID_NOT_FOUND = new()
    {
        HttpCode = StatusCodes.Status404NotFound,
        AppCode = $"G25.REMOVE_GUEST_HISTORY.{G25RemoveGuestHistoryItemResponseAppCode.GUEST_ID_NOT_FOUND}.{(int) G25RemoveGuestHistoryItemResponseAppCode.GUEST_ID_NOT_FOUND}",
    };

    public static readonly G25RemoveGuestHistoryHttpReponse DATABASE_ERROR = new()
    {
        HttpCode = StatusCodes.Status500InternalServerError,
        AppCode = $"G25.REMOVE_GUEST_HISTORY.{G25RemoveGuestHistoryItemResponseAppCode.DATABASE_ERROR}.{(int) G25RemoveGuestHistoryItemResponseAppCode.DATABASE_ERROR}",
    };

    public static G25RemoveGuestHistoryHttpReponse MapFrom(G25RemoveGuestHistoryItemReponse reponse)
    {
        if (reponse.AppCode == G25RemoveGuestHistoryItemResponseAppCode.SUCCESS)
        {
            return SUCCESS;
        }

        if (reponse.AppCode == G25RemoveGuestHistoryItemResponseAppCode.GUEST_ID_NOT_FOUND)
        {
            return GUEST_ID_NOT_FOUND;
        }

        return DATABASE_ERROR;
    }
}
