using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG35;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG35.DTOs;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG35.HttpResponses;

public sealed class G35HttpResponse : AppHttpResponse<G35UserProfileResponseDto>
{
    private static readonly object _lock = new object();
    private static G35HttpResponse _notFoundInstance;

    public static G35HttpResponse MapFrom(G35Response response)
    {
        if (response.AppCode == G35ResponseAppCode.SUCCESS)
        {
            return new()
            {
                HttpCode = StatusCodes.Status200OK,
                Body = G35UserProfileResponseDto.MapFrom(response)
            };
        }

        return USER_ID_NOT_FOUND();
    }

    public static G35HttpResponse USER_ID_NOT_FOUND()
    {
        lock (_lock)
        {
            if (_notFoundInstance == null)
            {
                _notFoundInstance = new()
                {
                    HttpCode = StatusCodes.Status404NotFound,
                };
            }
        }

        return _notFoundInstance;
    }
}
