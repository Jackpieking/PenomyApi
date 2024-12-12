using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt3.OtherHandlers.RestoreAllDeletedItems;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt3.HttpResponses;

public class Art3RestoreAllDeletedItemsHttpResponse : AppHttpResponse<object>
{
    public static readonly Art3RestoreAllDeletedItemsHttpResponse SUCCESS = new()
    {
        AppCode = GetAppCode(Art3RestoreAllDeletedItemsResponseAppCode.SUCCESS),
        HttpCode = StatusCodes.Status200OK,
    };

    public static readonly Art3RestoreAllDeletedItemsHttpResponse NO_DELETED_ITEMS_FOUND = new()
    {
        AppCode = GetAppCode(Art3RestoreAllDeletedItemsResponseAppCode.NO_DELETED_ITEMS_FOUND),
        HttpCode = StatusCodes.Status400BadRequest,
    };

    public static readonly Art3RestoreAllDeletedItemsHttpResponse DATABASE_ERROR = new()
    {
        AppCode = GetAppCode(Art3RestoreAllDeletedItemsResponseAppCode.DATABASE_ERROR),
        HttpCode = StatusCodes.Status500InternalServerError,
    };

    public static Art3RestoreAllDeletedItemsHttpResponse MapFrom(Art3RestoreAllDeletedItemsResponse response)
    {
        if (response.AppCode == Art3RestoreAllDeletedItemsResponseAppCode.SUCCESS)
        {
            return SUCCESS;
        }

        if (response.AppCode == Art3RestoreAllDeletedItemsResponseAppCode.NO_DELETED_ITEMS_FOUND)
        {
            return NO_DELETED_ITEMS_FOUND;
        }

        return DATABASE_ERROR;
    }

    private static string GetAppCode(Art3RestoreAllDeletedItemsResponseAppCode appCode)
    {
        return $"ART3.RESTORE_ALL_DELETED_ITEMS.{appCode}.{(int) appCode}";
    }
}
