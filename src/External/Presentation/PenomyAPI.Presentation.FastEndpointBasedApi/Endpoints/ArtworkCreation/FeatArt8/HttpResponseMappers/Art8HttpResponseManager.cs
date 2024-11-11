using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt8;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt8.HttpResponses;
using System;
using System.Collections.Concurrent;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt7.HttpResponseManagers;

public static class Art8HttpResponseMapper
{
    private static ConcurrentDictionary<
        Art8ResponseAppCode,
        Func<Art8Response, Art8HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: Art8ResponseAppCode.SUCCESS,
            value: (response) => new()
            {
                AppCode = Art8HttpResponse.GetAppCode(Art8ResponseAppCode.SUCCESS),
                HttpCode = StatusCodes.Status200OK,
                Body = null,
            });

        _dictionary.TryAdd(
            key: Art8ResponseAppCode.ARTWORK_ID_NOT_FOUND,
            value: (response) => new()
            {
                AppCode = Art8HttpResponse.GetAppCode(Art8ResponseAppCode.ARTWORK_ID_NOT_FOUND),
                HttpCode = StatusCodes.Status404NotFound,
            });

        _dictionary.TryAdd(
            key: Art8ResponseAppCode.ARTWORK_IS_ALREADY_REMOVED,
            value: (response) => new()
            {
                AppCode = Art8HttpResponse.GetAppCode(Art8ResponseAppCode.ARTWORK_IS_ALREADY_REMOVED),
                HttpCode = StatusCodes.Status400BadRequest,
            });

        _dictionary.TryAdd(
            key: Art8ResponseAppCode.DATABASE_ERROR,
            value: (response) => new()
            {
                AppCode = Art8HttpResponse.GetAppCode(Art8ResponseAppCode.DATABASE_ERROR),
                HttpCode = StatusCodes.Status500InternalServerError,
            });
    }

    internal static Func<Art8Response, Art8HttpResponse> Resolve(Art8ResponseAppCode statusCode)
    {
        if (Equals(objA: _dictionary, objB: default))
        {
            Init();
        }

        var keyExisted = _dictionary.TryGetValue(statusCode, out var value);

        if (keyExisted)
        {
            return value;
        }

        return _dictionary[Art8ResponseAppCode.ARTWORK_ID_NOT_FOUND];
    }
}
