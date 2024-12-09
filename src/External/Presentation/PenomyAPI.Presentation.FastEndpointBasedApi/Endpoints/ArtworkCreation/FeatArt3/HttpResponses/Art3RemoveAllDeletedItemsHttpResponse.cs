using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt3.OtherHandlers.RemoveAllDeteledItems;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt3.HttpResponses;

public class Art3RemoveAllDeletedItemsHttpResponse : AppHttpResponse<object>
{
    public static readonly Art3RemoveAllDeletedItemsHttpResponse SUCCESS = new()
    {
        AppCode = GetAppCode(Art3RemoveAllDeletedItemsResponseAppCode.SUCCESS),
        HttpCode = StatusCodes.Status200OK,
    };

    public static readonly Art3RemoveAllDeletedItemsHttpResponse NO_DELETED_ITEMS_FOUND = new()
    {
        AppCode = GetAppCode(Art3RemoveAllDeletedItemsResponseAppCode.NO_DELETED_ITEMS_FOUND),
        HttpCode = StatusCodes.Status400BadRequest,
    };

    public static readonly Art3RemoveAllDeletedItemsHttpResponse DATABASE_ERROR = new()
    {
        AppCode = GetAppCode(Art3RemoveAllDeletedItemsResponseAppCode.DATABASE_ERROR),
        HttpCode = StatusCodes.Status500InternalServerError,
    };

    public static Art3RemoveAllDeletedItemsHttpResponse MapFrom(Art3RemoveAllDeletedItemsResponse response)
    {
        if (response.AppCode == Art3RemoveAllDeletedItemsResponseAppCode.SUCCESS)
        {
            return SUCCESS;
        }

        if (response.AppCode == Art3RemoveAllDeletedItemsResponseAppCode.NO_DELETED_ITEMS_FOUND)
        {
            return NO_DELETED_ITEMS_FOUND;
        }

        return DATABASE_ERROR;
    }

    private static string GetAppCode(Art3RemoveAllDeletedItemsResponseAppCode appCode)
    {
        return $"ART3.REMOVE_ALL_DELETED_ITEMS.{appCode}.{(int) appCode}";
    }
}
