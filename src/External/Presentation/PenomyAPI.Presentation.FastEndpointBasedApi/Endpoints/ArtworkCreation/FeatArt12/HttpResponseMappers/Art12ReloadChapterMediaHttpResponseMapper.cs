using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt12.OtherHandlers.ReloadChapterMedias;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt12.DTOs.GetChapterDetail;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt12.HttpResponses;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt12.HttpResponseMappers;

public static class Art12ReloadChapterMediaHttpResponseMapper
{
    private static ConcurrentDictionary<
        Art12ReloadChapterMediaResponseAppCode,
        Func<Art12ReloadChapterMediaResponse, Art12ReloadChapterMediaHttpResponse>> _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: Art12ReloadChapterMediaResponseAppCode.SUCCESS,
            value: (response) => new()
            {
                AppCode = Art12ReloadChapterMediaHttpResponse.GetAppCode(Art12ReloadChapterMediaResponseAppCode.SUCCESS),
                HttpCode = StatusCodes.Status200OK,
                Body = response.ChapterMedias.Select(Art12ChapterMediaItemResponseDto.MapFrom)
            });

        _dictionary.TryAdd(
            key: Art12ReloadChapterMediaResponseAppCode.DATABASE_ERROR,
            value: (response) => new()
            {
                AppCode = Art12ReloadChapterMediaHttpResponse.GetAppCode(Art12ReloadChapterMediaResponseAppCode.DATABASE_ERROR),
                HttpCode = StatusCodes.Status500InternalServerError,
            });

        _dictionary.TryAdd(
            key: Art12ReloadChapterMediaResponseAppCode.CHAPTER_IS_NOT_FOUND,
            value: (response) => new()
            {
                AppCode = Art12ReloadChapterMediaHttpResponse.GetAppCode(Art12ReloadChapterMediaResponseAppCode.CHAPTER_IS_NOT_FOUND),
                HttpCode = StatusCodes.Status404NotFound,
            });
    }

    private static Func<Art12ReloadChapterMediaResponse, Art12ReloadChapterMediaHttpResponse> Resolve(
        Art12ReloadChapterMediaResponseAppCode statusCode)
    {
        if (Equals(objA: _dictionary, objB: default))
        {
            Init();
        }

        if (_dictionary.TryGetValue(statusCode, out var response))
        {
            return response;
        }

        return _dictionary[Art12ReloadChapterMediaResponseAppCode.CHAPTER_IS_NOT_FOUND];
    }

    internal static Art12ReloadChapterMediaHttpResponse Map(
        Art12ReloadChapterMediaResponse featureResponse)
    {
        return Resolve(featureResponse.AppCode).Invoke(featureResponse);
    }
}
