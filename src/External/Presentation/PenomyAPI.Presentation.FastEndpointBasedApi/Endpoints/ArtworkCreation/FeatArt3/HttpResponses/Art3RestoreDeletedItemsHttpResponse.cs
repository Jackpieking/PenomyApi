using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt3.OtherHandlers.RestoreDeletedItems;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt3.HttpResponses;

public class Art3RestoreDeletedItemsHttpResponse : AppHttpResponse<object>
{
    public static readonly Art3RestoreDeletedItemsHttpResponse SUCCESS = new()
    {
        HttpCode = StatusCodes.Status200OK,
    };

    public static readonly Art3RestoreDeletedItemsHttpResponse CREATOR_HAS_NO_PERMISSION = new()
    {
        HttpCode = StatusCodes.Status403Forbidden,
    };

    public static readonly Art3RestoreDeletedItemsHttpResponse DATABASE_ERROR = new()
    {
        HttpCode = StatusCodes.Status500InternalServerError,
    };

    public static Art3RestoreDeletedItemsHttpResponse MapFrom(Art3RestoreDeletedItemResponse response)
    {
        if (response.AppCode == Art3RestoreDeletedItemsResponseAppCode.SUCCESS)
        {
            return SUCCESS;
        }

        if (response.AppCode == Art3RestoreDeletedItemsResponseAppCode.CREATOR_HAS_NO_PERMISSION)
        {
            return CREATOR_HAS_NO_PERMISSION;
        }

        return DATABASE_ERROR;
    }
}
