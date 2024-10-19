using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt10;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt10.HttpResponses;
using System;
using System.Collections.Concurrent;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt10.HttpResponseManagers;

public static class Art10HttpResponseManager
{
    private static ConcurrentDictionary<Art10ResponseAppCode, Func<Art10Response, Art10HttpResponse>> _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: Art10ResponseAppCode.SUCCESS,
            value: (response) => new()
            {
                AppCode = Art10HttpResponse.GetAppCode(Art10ResponseAppCode.SUCCESS),
                HttpCode = StatusCodes.Status200OK,
            });

        _dictionary.TryAdd(
           key: Art10ResponseAppCode.FILE_SERVICE_ERROR,
           value: (response) => new()
           {
               AppCode = Art10HttpResponse.GetAppCode(Art10ResponseAppCode.FILE_SERVICE_ERROR),
               HttpCode = StatusCodes.Status500InternalServerError,
           });

        _dictionary.TryAdd(
           key: Art10ResponseAppCode.DATABASE_ERROR,
           value: (response) => new()
           {
               AppCode = Art10HttpResponse.GetAppCode(Art10ResponseAppCode.DATABASE_ERROR),
               HttpCode = StatusCodes.Status500InternalServerError,
           });

        _dictionary.TryAdd(
           key: Art10ResponseAppCode.INVALID_FILE_EXTENSION,
           value: (response) => new()
           {
               AppCode = Art10HttpResponse.GetAppCode(Art10ResponseAppCode.INVALID_FILE_EXTENSION),
               HttpCode = StatusCodes.Status400BadRequest,
           });

        _dictionary.TryAdd(
           key: Art10ResponseAppCode.INVALID_FILE_FORMAT,
           value: (response) => new()
           {
               AppCode = Art10HttpResponse.GetAppCode(Art10ResponseAppCode.INVALID_FILE_FORMAT),
               HttpCode = StatusCodes.Status400BadRequest,
           });

        _dictionary.TryAdd(
           key: Art10ResponseAppCode.FILE_SIZE_IS_EXCEED_THE_LIMIT,
           value: (response) => new()
           {
               AppCode = Art10HttpResponse.GetAppCode(Art10ResponseAppCode.FILE_SIZE_IS_EXCEED_THE_LIMIT),
               HttpCode = StatusCodes.Status400BadRequest,
           });

        _dictionary.TryAdd(
           key: Art10ResponseAppCode.CHAPTER_IMAGE_LIST_IS_EMPTY,
           value: (response) => new()
           {
               AppCode = Art10HttpResponse.GetAppCode(Art10ResponseAppCode.CHAPTER_IMAGE_LIST_IS_EMPTY),
               HttpCode = StatusCodes.Status400BadRequest,
           });
    }

    internal static Func<Art10Response, Art10HttpResponse> Resolve(Art10ResponseAppCode appCode)
    {
        if (Equals(objA: _dictionary, objB: default))
        {
            Init();
        }

        if (_dictionary.TryGetValue(appCode, out var response))
        {
            return response;
        }

        return _dictionary[Art10ResponseAppCode.INVALID_FILE_FORMAT];
    }
}
