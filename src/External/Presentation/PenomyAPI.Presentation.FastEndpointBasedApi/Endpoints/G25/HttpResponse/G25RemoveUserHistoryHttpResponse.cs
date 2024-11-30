using Microsoft.AspNetCore.Http;
using PenomyAPI.App.G25.OtherHandlers.RemoveUserHistoryItem;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25.HttpResponse;

public class G25RemoveUserHistoryHttpResponse : AppHttpResponse<object>
{
    public static readonly G25RemoveUserHistoryHttpResponse SUCCESS = new()
    {
        HttpCode = StatusCodes.Status200OK,
        AppCode = $"G25.REMOVE_USER_HISTORY.{G25RemoveUserHistoryItemResponseAppCode.SUCCESS}.{(int) G25RemoveUserHistoryItemResponseAppCode.SUCCESS}",
    };

    public static readonly G25RemoveUserHistoryHttpResponse DATABASE_ERROR = new()
    {
        HttpCode = StatusCodes.Status500InternalServerError,
        AppCode = $"G25.REMOVE_USER_HISTORY.{G25RemoveUserHistoryItemResponseAppCode.DATABASE_ERROR}.{(int) G25RemoveUserHistoryItemResponseAppCode.DATABASE_ERROR}",
    };

    public static G25RemoveUserHistoryHttpResponse MapFrom(G25RemoveUserHistoryItemReponse reponse)
    {
        if (reponse.AppCode == G25RemoveUserHistoryItemResponseAppCode.SUCCESS)
        {
            return SUCCESS;
        }

        return DATABASE_ERROR;
    }
}
