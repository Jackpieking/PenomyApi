using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;
using System;
using static PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25.HttpResponse.G25InitGuestViewHistoryHttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25.HttpResponse;

public class G25InitGuestViewHistoryHttpResponse
    : AppHttpResponse<G25InitGuestViewHistoryHttpResponseBody>
{
    public class G25InitGuestViewHistoryHttpResponseBody
    {
        public string GuestId { get; set; }

        public DateTime LastActiveAt { get; set; }
    }
}
