using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt7.OtherHandlers.LoadComicDetail;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt7.HttpResponse;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt7.HttpResponseManagers;

public static class Art7LoadComicDetailHttpResponseManager
{
    private static ConcurrentDictionary<
        Art7LoadComicDetailResponseStatusCode,
        Func<Art7LoadComicDetailResponse, Art7LoadComicDetailHttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: Art7LoadComicDetailResponseStatusCode.SUCCESS,
            value: (response) =>
                new()
                {
                    AppCode = Art7LoadComicDetailHttpResponse.GetAppCode(
                        Art7LoadComicDetailResponseStatusCode.SUCCESS
                    ),
                    HttpCode = StatusCodes.Status200OK,
                    Body = new DTOs.Art7LoadComicDetailResponseDto
                    {
                        Id = response.ComicDetail.Id.ToString(),
                        Title = response.ComicDetail.Title,
                        Introduction = response.ComicDetail.Introduction,
                        ThumbnailUrl = response.ComicDetail.ThumbnailUrl,
                        OriginId = response.ComicDetail.ArtworkOriginId.ToString(),
                        PublicLevel = response.ComicDetail.PublicLevel,
                        ArtworkStatus = response.ComicDetail.ArtworkStatus,
                        UpdatedAt = response.ComicDetail.UpdatedAt,
                        SelectedCategories = response.ComicDetail.ArtworkCategories.Select(
                            artworkCategory => new DTOs.Art7ArtworkCategoryDto
                            {
                                ArtworkId = artworkCategory.ArtworkId.ToString(),
                                CategoryId = artworkCategory.CategoryId.ToString(),
                            }
                        ),
                        AllowComment = response.ComicDetail.AllowComment
                    },
                }
        );

        _dictionary.TryAdd(
            key: Art7LoadComicDetailResponseStatusCode.ID_NOT_FOUND,
            value: (response) =>
                new()
                {
                    AppCode = Art7LoadComicDetailHttpResponse.GetAppCode(
                        Art7LoadComicDetailResponseStatusCode.ID_NOT_FOUND
                    ),
                    HttpCode = StatusCodes.Status404NotFound,
                }
        );

        _dictionary.TryAdd(
            key: Art7LoadComicDetailResponseStatusCode.CREATOR_HAS_NO_PERMISSION,
            value: (response) =>
                new()
                {
                    AppCode = Art7LoadComicDetailHttpResponse.GetAppCode(
                        Art7LoadComicDetailResponseStatusCode.CREATOR_HAS_NO_PERMISSION
                    ),
                    HttpCode = StatusCodes.Status403Forbidden,
                }
        );
    }

    internal static Func<Art7LoadComicDetailResponse, Art7LoadComicDetailHttpResponse> Resolve(
        Art7LoadComicDetailResponseStatusCode statusCode
    )
    {
        if (Equals(objA: _dictionary, objB: default))
        {
            Init();
        }

        return _dictionary[statusCode];
    }
}
