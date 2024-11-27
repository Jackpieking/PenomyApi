using Microsoft.AspNetCore.Http;
using PenomyAPI.App.G25.OtherHandlers.GetGuestTracking;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25.HttpResponse;

public sealed class G25GetGuestTrackingHttpResponse
    : AppHttpResponse<G25GetGuestTrackingResponse>
{
    public static G25GetGuestTrackingHttpResponse MapFrom(G25GetGuestTrackingResponse response)
    {
        if (response.AppCode == G25GetGuestTrackingResponseAppCode.SUCCESS)
        {
            return SUCCESS(response);
        }

        return GUEST_ID_NOT_FOUND;
    }

    public static G25GetGuestTrackingHttpResponse SUCCESS(G25GetGuestTrackingResponse response)
    {
        return new()
        {
            HttpCode = StatusCodes.Status200OK,
            AppCode = response.AppCode.ToString(),
            Body = response
        };
    }

    public static readonly G25GetGuestTrackingHttpResponse GUEST_ID_NOT_FOUND = new()
    {
        HttpCode = StatusCodes.Status404NotFound,
    };
}
