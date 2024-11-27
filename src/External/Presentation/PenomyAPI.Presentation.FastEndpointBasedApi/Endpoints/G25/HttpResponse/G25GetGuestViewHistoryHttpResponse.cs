using Microsoft.AspNetCore.Http;
using PenomyAPI.App.G25.OtherHandlers.GetGuestHistory;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25.HttpResponse;

public sealed class G25GetGuestViewHistoryHttpResponse
    : AppHttpResponse<IEnumerable<G25ArtworkViewHistoryItemResponseDto>>
{
    public static readonly G25GetGuestViewHistoryHttpResponse EMPTY_VIEW_HISTORY = new()
    {
        HttpCode = StatusCodes.Status200OK,
        Body = new List<G25ArtworkViewHistoryItemResponseDto>(),
    };

    public static readonly G25GetGuestViewHistoryHttpResponse GUEST_ID_NOT_FOUND = new()
    {
        HttpCode = StatusCodes.Status404NotFound,
    };

    public static G25GetGuestViewHistoryHttpResponse SUCCESS(G25GetGuestHistoryResponse response)
        => new()
        {
            HttpCode = StatusCodes.Status200OK,
            Body = response.ViewedArtworks.Select(G25ArtworkViewHistoryItemResponseDto.MapFrom),
        };

    public static G25GetGuestViewHistoryHttpResponse MapFrom(G25GetGuestHistoryResponse response)
    {
        if (response.AppCode == G25GetGuestHistoryResponseAppCode.SUCCESS)
        {
            return SUCCESS(response);
        }

        if (response.AppCode == G25GetGuestHistoryResponseAppCode.EMPTY_VIEW_HISTORY)
        {
            return EMPTY_VIEW_HISTORY;
        }

        return GUEST_ID_NOT_FOUND;
    }
}
