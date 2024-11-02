using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt12.OtherHandlers.GetChapterDetail;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt12.DTOs.GetChapterDetail;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt12.HttpResponses;
using System;
using System.Collections.Concurrent;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt12.HttpResponseMappers;

public static class Art12GetChapterDetailHttpResponseMapper
{
    private static ConcurrentDictionary<
        Art12GetChapterDetailResponseAppCode,
        Func<Art12GetChapterDetailResponse, Art12GetChapterDetailHttpResponse>> _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: Art12GetChapterDetailResponseAppCode.SUCCESS,
            value: (response) => new()
            {
                AppCode = Art12GetChapterDetailHttpResponse.GetAppCode(Art12GetChapterDetailResponseAppCode.SUCCESS),
                HttpCode = StatusCodes.Status200OK,
                Body = Art12GetChapterDetailResponseDto.MapFrom(response.ChapterDetail)
            });

        _dictionary.TryAdd(
            key: Art12GetChapterDetailResponseAppCode.DATABASE_ERROR,
            value: (response) => new()
            {
                AppCode = Art12GetChapterDetailHttpResponse.GetAppCode(Art12GetChapterDetailResponseAppCode.DATABASE_ERROR),
                HttpCode = StatusCodes.Status500InternalServerError,
            });

        _dictionary.TryAdd(
            key: Art12GetChapterDetailResponseAppCode.CHAPTER_IS_NOT_FOUND,
            value: (response) => new()
            {
                AppCode = Art12GetChapterDetailHttpResponse.GetAppCode(Art12GetChapterDetailResponseAppCode.CHAPTER_IS_NOT_FOUND),
                HttpCode = StatusCodes.Status404NotFound,
            });

        _dictionary.TryAdd(
            key: Art12GetChapterDetailResponseAppCode.CHAPTER_IS_TEMPORARILY_REMOVED,
            value: (response) => new()
            {
                AppCode = Art12GetChapterDetailHttpResponse.GetAppCode(Art12GetChapterDetailResponseAppCode.CHAPTER_IS_TEMPORARILY_REMOVED),
                HttpCode = StatusCodes.Status400BadRequest,
            });

        _dictionary.TryAdd(
            key: Art12GetChapterDetailResponseAppCode.NO_PERMISSION_GRANTED_FOR_CURRENT_CREATOR,
            value: (response) => new()
            {
                AppCode = Art12GetChapterDetailHttpResponse.GetAppCode(Art12GetChapterDetailResponseAppCode.NO_PERMISSION_GRANTED_FOR_CURRENT_CREATOR),
                HttpCode = StatusCodes.Status401Unauthorized,
            });
    }

    private static Func<Art12GetChapterDetailResponse, Art12GetChapterDetailHttpResponse> Resolve(
        Art12GetChapterDetailResponseAppCode statusCode)
    {
        if (Equals(objA: _dictionary, objB: default))
        {
            Init();
        }

        if (_dictionary.TryGetValue(statusCode, out var response))
        {
            return response;
        }

        return _dictionary[Art12GetChapterDetailResponseAppCode.CHAPTER_IS_NOT_FOUND];
    }

    internal static Art12GetChapterDetailHttpResponse Map(Art12GetChapterDetailResponse featureResponse)
    {
        return Resolve(featureResponse.AppCode).Invoke(featureResponse);
    }
}
