using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG5.OtherHandlers.UserPreferences;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5_Support.DTOs;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5_Support.HttpResponses;

public class G5UserPreferenceHttpResponse : AppHttpResponse<G5UserPreferenceResponseDto>
{
    public static G5UserPreferenceHttpResponse MapFrom(G5UserPreferenceResponse response)
    {
        var userArtworkPreference = response.UserArtworkPreference;

        return new()
        {
            HttpCode = StatusCodes.Status200OK,
            Body = new()
            {
                FirstChapterId = userArtworkPreference.FirstChapterId.ToString(),
                LastReadChapterId = userArtworkPreference.LastReadChapterId.ToString(),
                HasFollowed = userArtworkPreference.HasFollowed,
                IsUserFavorite = userArtworkPreference.IsUserFavorite,
            }
        };
    }
}
