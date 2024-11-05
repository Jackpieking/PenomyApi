using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG35;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG35.DTOs;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG35.HttpResponses;

public sealed class G35HttpResponse : AppHttpResponse<G35UserProfileResponseDto>
{
    private static readonly object _lock = new object();
    private static G35HttpResponse _unauthorizedInstance;

    public static G35HttpResponse MapFrom(G35Response response)
    {
        return new()
        {
            HttpCode = StatusCodes.Status200OK,
            Body = G35UserProfileResponseDto.MapFrom(response)
        };
    }

    public static G35HttpResponse UNAUTHORIZED()
    {
        lock (_lock)
        {
            if (_unauthorizedInstance == null)
            {
                _unauthorizedInstance = new()
                {
                    HttpCode = StatusCodes.Status401Unauthorized,
                };
            }
        }

        return _unauthorizedInstance;
    }
}
