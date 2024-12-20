using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt3.OtherHandlers.CheckDeletedItems;
using PenomyAPI.Domain.RelationalDb.Models.ArtworkCreation.FeatArt3;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt3.HttpResponses;

public class Art3CheckDeletedItemsHttpResponse : AppHttpResponse<Art3CheckDeletedItemReadModel>
{
    public static Art3CheckDeletedItemsHttpResponse MapFrom(Art3CheckDeletedItemsResponse response)
    {
        return new()
        {
            HttpCode = StatusCodes.Status200OK,
            Body = response.Result,
        };
    }
}
