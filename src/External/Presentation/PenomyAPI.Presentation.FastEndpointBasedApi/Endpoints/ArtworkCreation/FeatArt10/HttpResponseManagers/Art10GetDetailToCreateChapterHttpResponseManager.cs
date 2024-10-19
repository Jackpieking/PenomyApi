using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt10.OtherHandlers.GetDetailToCreateChapter;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt10.HttpResponses;
using System;
using System.Collections.Concurrent;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt10.HttpResponseManagers;

public static class Art10GetDetailToCreateChapterHttpResponseManager
{
    private static ConcurrentDictionary<
        Art10GetDetailToCreateChapterResponseAppCode,
        Func<Art10GetDetailToCreateChapterResponse, Art10GetDetailToCreateChapterHttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: Art10GetDetailToCreateChapterResponseAppCode.SUCCESS,
            value: (response) => new()
            {
                AppCode = Art10GetDetailToCreateChapterHttpResponse.GetAppCode(response.AppCode),
                HttpCode = StatusCodes.Status200OK,
                Body = new DTOs.ComicDetailToCreateChapterResponseDto
                {
                    Title = response.ComicDetail.Title,
                    LastChapterUploadOrder = response.ComicDetail.LastChapterUploadOrder,
                }
            });

        _dictionary.TryAdd(
            key: Art10GetDetailToCreateChapterResponseAppCode.COMIC_ID_NOT_FOUND,
            value: (response) => new()
            {
                AppCode = Art10GetDetailToCreateChapterHttpResponse.GetAppCode(response.AppCode),
                HttpCode = StatusCodes.Status404NotFound,
            });
    }

    internal static Func<Art10GetDetailToCreateChapterResponse, Art10GetDetailToCreateChapterHttpResponse> Resolve(Art10GetDetailToCreateChapterResponseAppCode appCode)
    {
        if (Equals(objA: _dictionary, objB: default))
        {
            Init();
        }

        if (_dictionary.TryGetValue(appCode, out var response))
        {
            return response;
        }

        return _dictionary[Art10GetDetailToCreateChapterResponseAppCode.COMIC_ID_NOT_FOUND];
    }
}
