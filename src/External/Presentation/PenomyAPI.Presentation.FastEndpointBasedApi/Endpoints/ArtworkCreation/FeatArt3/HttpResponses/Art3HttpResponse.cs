using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt3;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt3.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;
using System.Collections.Generic;
using System.Linq;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt3.HttpResponses;

public class Art3HttpResponse : AppHttpResponse<IEnumerable<Art3DeletedArtworkItemResponseDto>>
{
    public static readonly Art3HttpResponse Empty = new()
    {
        AppCode = "ART3.GET_DELETED_ITEMS.EMPTY.2",
        Body = [],
    };

    public static Art3HttpResponse MapFrom(Art3Response response)
    {
        if (response.IsEmpty)
        {
            return Empty;
        }

        return new()
        {
            AppCode = "ART3.GET_DELETED_ITEMS.SUCCESS.1",
            HttpCode = StatusCodes.Status200OK,
            Body = response.DeletedItems.Select(Art3DeletedArtworkItemResponseDto.MapFrom)
        };
    }
}
