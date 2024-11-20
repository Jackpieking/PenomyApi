using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG35.OtherHandlers.GetCreatorProfile;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG35.DTOs;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG35.HttpResponses;

public sealed class G35GetCreatorProfileHttpResponse : AppHttpResponse<G35CreatorProfileResponseDto>
{
    private static readonly object _lock = new object();
    private static G35GetCreatorProfileHttpResponse _notFoundInstance;

    public static G35GetCreatorProfileHttpResponse MapFrom(G35GetCreatorProfileResponse response)
    {
        if (response.AppCode == G35GetCreatorProfileResponseAppCode.SUCCESS)
        {
            return new()
            {
                HttpCode = StatusCodes.Status200OK,
                Body = G35CreatorProfileResponseDto.MapFrom(response)
            };
        }

        return USER_ID_NOT_FOUND();
    }

    public static G35GetCreatorProfileHttpResponse USER_ID_NOT_FOUND()
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
