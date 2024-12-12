using Microsoft.AspNetCore.Http;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G61.DTOs;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G61.HttpResponse;

public class G61CheckHasFollowHttpResponse : AppHttpResponse<G61CheckHasFollowResponseDto>
{
    public static G61CheckHasFollowHttpResponse HAS_NOT_FOLLOWED = new()
    {
        HttpCode = StatusCodes.Status200OK,
        Body = new()
        {
            HasFollowed = false,
        }
    };
}
