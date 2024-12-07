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
            HttpCode = StatusCodes.Status200OK,
            Body = response.DeletedItems.Select(Art3DeletedArtworkItemResponseDto.MapFrom)
        };
    }
}
