using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt3.OtherHandlers.RemoveDeletedItems;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt3.HttpResponses;

public class Art3RemoveDeletedItemsHttpResponse : AppHttpResponse<object>
{
    public static readonly Art3RemoveDeletedItemsHttpResponse SUCCESS = new()
    {
        AppCode = GetAppCode(Art3RemoveDeletedItemsResponseAppCode.SUCCESS),
        HttpCode = StatusCodes.Status200OK,
    };

    public static readonly Art3RemoveDeletedItemsHttpResponse CREATOR_HAS_NO_PERMISSION = new()
    {
        AppCode = GetAppCode(Art3RemoveDeletedItemsResponseAppCode.CREATOR_HAS_NO_PERMISSION),
        HttpCode = StatusCodes.Status403Forbidden,
    };

    public static readonly Art3RemoveDeletedItemsHttpResponse DATABASE_ERROR = new()
    {
        AppCode = GetAppCode(Art3RemoveDeletedItemsResponseAppCode.DATABASE_ERROR),
        HttpCode = StatusCodes.Status500InternalServerError,
    };

    public static Art3RemoveDeletedItemsHttpResponse MapFrom(Art3RemoveDeletedItemsResponse response)
    {
        if (response.AppCode == Art3RemoveDeletedItemsResponseAppCode.SUCCESS)
        {
            return SUCCESS;
        }

        if (response.AppCode == Art3RemoveDeletedItemsResponseAppCode.CREATOR_HAS_NO_PERMISSION)
        {
            return CREATOR_HAS_NO_PERMISSION;
        }

        return DATABASE_ERROR;
    }

    private static string GetAppCode(Art3RemoveDeletedItemsResponseAppCode appCode)
    {
        return $"ART3.REMOVE_DELETED_ITEMS.{appCode}.{(int) appCode}";
    }
}
