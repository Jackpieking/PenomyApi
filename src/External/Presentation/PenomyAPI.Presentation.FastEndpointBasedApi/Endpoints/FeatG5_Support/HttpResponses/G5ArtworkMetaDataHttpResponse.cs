using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG5.OtherHandlers.GetArtworkMetaData;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5_Support.DTOs;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5_Support.HttpResponses;

public class G5ArtworkMetaDataHttpResponse : AppHttpResponse<G5ArtworkMetaDataResponseDto>
{
    public static string GetAppCode(G5ArtworkMetaDataResponseAppCode appCode)
    {
        return $"G5.ARTWORK_METADATA.{appCode}.{(int) appCode}";
    }

    public static readonly G5ArtworkMetaDataHttpResponse ARTWORK_IS_NOT_FOUND = new()
    {
        HttpCode = StatusCodes.Status404NotFound,
        AppCode = GetAppCode(G5ArtworkMetaDataResponseAppCode.ARTWORK_IS_NOT_FOUND)
    };

    public static G5ArtworkMetaDataHttpResponse SUCCESS(G5ArtworkMetaDataResponse response)
    {
        var metadata = response.ArtworkMetaData;

        return new()
        {
            HttpCode = StatusCodes.Status200OK,
            AppCode = GetAppCode(G5ArtworkMetaDataResponseAppCode.SUCCESS),
            Body = new()
            {
                FavoriteCount = metadata.TotalFavorites,
                FollowCount = metadata.TotalFollowers,
                StarRates = metadata.GetAverageStarRate(),
                TotalUsersRated = metadata.TotalUsersRated,
                ViewCount = metadata.TotalViews,
            },
        };
    }

    public static G5ArtworkMetaDataHttpResponse MapFrom(G5ArtworkMetaDataResponse response)
    {
        if (response.AppCode == G5ArtworkMetaDataResponseAppCode.SUCCESS)
        {
            return SUCCESS(response);
        }

        return ARTWORK_IS_NOT_FOUND;
    }
}
