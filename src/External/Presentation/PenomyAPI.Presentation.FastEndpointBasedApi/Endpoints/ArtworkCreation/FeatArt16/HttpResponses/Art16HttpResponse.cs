using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt16;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.Common.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt16.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;
using System.Linq;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt16.HttpResponses;

public class Art16HttpResponse : AppHttpResponse<AnimeDetailResponseDto>
{
    public static string GetAppCode(Art16ResponseAppCode appCode)
    {
        return $"Art16.{appCode}.{(int) appCode}";
    }

    public static Art16HttpResponse SUCCESS(Art16Response response)
    {
        return new()
        {
            AppCode = GetAppCode(Art16ResponseAppCode.SUCCESS),
            HttpCode = StatusCodes.Status200OK,
            Body = new()
            {
                Id = response.AnimeDetail.Id.ToString(),
                Title = response.AnimeDetail.Title,
                ThumbnailUrl = response.AnimeDetail.ThumbnailUrl,
                Introduction = response.AnimeDetail.Introduction,
                ArtworkStatus = response.AnimeDetail.ArtworkStatus,
                AuthorName = response.AnimeDetail.Creator.NickName,
                Origin = response.AnimeDetail.Origin.CountryName,
                Categories = response.AnimeDetail.ArtworkCategories.Select(category => new CategoryDto
                {
                    Id = category.CategoryId.ToString(),
                    Label = category.Category.Name
                }),
                Series = SeriesDto.MapFrom(response.AnimeDetail.ArtworkSeries)
            }
        };
    }

    public static readonly Art16HttpResponse ARTWORK_ID_NOT_FOUND = new()
    {
        AppCode = GetAppCode(Art16ResponseAppCode.ARTWORK_ID_NOT_FOUND),
        HttpCode = StatusCodes.Status404NotFound,
    };

    public static readonly Art16HttpResponse ARTWORK_IS_TEMPORARILY_REMOVED = new()
    {
        AppCode = GetAppCode(Art16ResponseAppCode.ARTWORK_IS_TEMPORARILY_REMOVED),
        HttpCode = StatusCodes.Status404NotFound,
    };

    public static readonly Art16HttpResponse ARTWORK_IS_NOT_AUTHORIZED_TO_CURRENT_CREATOR = new()
    {
        AppCode = GetAppCode(Art16ResponseAppCode.ARTWORK_IS_NOT_AUTHORIZED_TO_CURRENT_CREATOR),
        HttpCode = StatusCodes.Status403Forbidden,
    };

    public static Art16HttpResponse MapFrom(Art16Response response)
    {
        switch (response.AppCode)
        {
            case Art16ResponseAppCode.SUCCESS:
                return SUCCESS(response);

            case Art16ResponseAppCode.ARTWORK_ID_NOT_FOUND:
                return ARTWORK_ID_NOT_FOUND;

            case Art16ResponseAppCode.ARTWORK_IS_NOT_AUTHORIZED_TO_CURRENT_CREATOR:
                return ARTWORK_IS_NOT_AUTHORIZED_TO_CURRENT_CREATOR;

            case Art16ResponseAppCode.ARTWORK_IS_TEMPORARILY_REMOVED:
                return ARTWORK_IS_TEMPORARILY_REMOVED;

            case Art16ResponseAppCode.DATABASE_ERROR:
            default:
                return ARTWORK_ID_NOT_FOUND;
        }
    }
}
