using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt12;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt12.HttpResponses;
using System;
using System.Collections.Concurrent;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt12.HttpResponseMappers;

public static class Art12HttpResponseMapper
{
    private static ConcurrentDictionary<
        Art12ResponseAppCode,
        Func<Art12Response, Art12HttpResponse>> _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: Art12ResponseAppCode.SUCCESS,
            value: (response) => new()
            {
                AppCode = Art12HttpResponse.GetAppCode(Art12ResponseAppCode.SUCCESS),
                HttpCode = StatusCodes.Status200OK
            });

        _dictionary.TryAdd(
            key: Art12ResponseAppCode.FILE_SERVICE_ERROR,
            value: (response) => new()
            {
                AppCode = Art12HttpResponse.GetAppCode(Art12ResponseAppCode.FILE_SERVICE_ERROR),
                HttpCode = StatusCodes.Status500InternalServerError,
            });

        _dictionary.TryAdd(
            key: Art12ResponseAppCode.INVALID_JSON_SCHEMA_FROM_INPUT_MEDIA_ITEMS,
            value: (response) => new()
            {
                AppCode = Art12HttpResponse.GetAppCode(Art12ResponseAppCode.INVALID_JSON_SCHEMA_FROM_INPUT_MEDIA_ITEMS),
                HttpCode = StatusCodes.Status400BadRequest,
            });

        _dictionary.TryAdd(
            key: Art12ResponseAppCode.INVALID_FILE_FORMAT,
            value: (response) => new()
            {
                AppCode = Art12HttpResponse.GetAppCode(Art12ResponseAppCode.INVALID_FILE_FORMAT),
                HttpCode = StatusCodes.Status400BadRequest,
            });

        _dictionary.TryAdd(
            key: Art12ResponseAppCode.FILE_SIZE_IS_EXCEED_THE_LIMIT,
            value: (response) => new()
            {
                AppCode = Art12HttpResponse.GetAppCode(Art12ResponseAppCode.FILE_SIZE_IS_EXCEED_THE_LIMIT),
                HttpCode = StatusCodes.Status400BadRequest,
            });

        _dictionary.TryAdd(
            key: Art12ResponseAppCode.CHAPTER_IS_TEMPORARILY_REMOVED,
            value: (response) => new()
            {
                AppCode = Art12HttpResponse.GetAppCode(Art12ResponseAppCode.CHAPTER_IS_TEMPORARILY_REMOVED),
                HttpCode = StatusCodes.Status400BadRequest,
            });

        _dictionary.TryAdd(
            key: Art12ResponseAppCode.NO_PERMISSION_GRANTED_FOR_CURRENT_CREATOR,
            value: (response) => new()
            {
                AppCode = Art12HttpResponse.GetAppCode(Art12ResponseAppCode.NO_PERMISSION_GRANTED_FOR_CURRENT_CREATOR),
                HttpCode = StatusCodes.Status401Unauthorized,
            });

        _dictionary.TryAdd(
            key: Art12ResponseAppCode.DATABASE_ERROR,
            value: (response) => new()
            {
                AppCode = Art12HttpResponse.GetAppCode(Art12ResponseAppCode.DATABASE_ERROR),
                HttpCode = StatusCodes.Status500InternalServerError,
            });
    }

    private static Func<Art12Response, Art12HttpResponse> Resolve(
        Art12ResponseAppCode statusCode)
    {
        if (Equals(objA: _dictionary, objB: default))
        {
            Init();
        }

        if (_dictionary.TryGetValue(statusCode, out var response))
        {
            return response;
        }

        return _dictionary[Art12ResponseAppCode.FILE_SERVICE_ERROR];
    }

    internal static Art12HttpResponse Map(Art12Response featureResponse)
    {
        return Resolve(featureResponse.AppCode).Invoke(featureResponse);
    }
}
