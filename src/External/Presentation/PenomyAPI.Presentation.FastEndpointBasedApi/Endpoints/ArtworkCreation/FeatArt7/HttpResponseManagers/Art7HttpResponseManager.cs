﻿using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt7;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt7.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt7.HttpResponseManagers;

public static class Art7HttpResponseManager
{
    private static ConcurrentDictionary<
        Art7ResponseStatusCode,
        Func<Art7Response, Art7HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: Art7ResponseStatusCode.SUCCESS,
            value: (response) => new()
            {
                AppCode = Art7HttpResponse.GetAppCode(Art7ResponseStatusCode.SUCCESS),
                HttpCode = StatusCodes.Status200OK,
                Body = null,
            });

        _dictionary.TryAdd(
            key: Art7ResponseStatusCode.DATABASE_ERROR,
            value: (response) => new()
            {
                AppCode = Art7HttpResponse.GetAppCode(Art7ResponseStatusCode.DATABASE_ERROR),
                HttpCode = StatusCodes.Status500InternalServerError,
            });

        _dictionary.TryAdd(
             key: Art7ResponseStatusCode.FILE_SERVICE_ERROR,
             value: (response) => new()
             {
                 AppCode = Art7HttpResponse.GetAppCode(Art7ResponseStatusCode.FILE_SERVICE_ERROR),
                 HttpCode = StatusCodes.Status500InternalServerError,
             });

        _dictionary.TryAdd(
            key: Art7ResponseStatusCode.INVALID_JSON_SCHEMA_FROM_INPUT_CATEGORIES,
            value: (response) =>
                new()
                {
                    AppCode = Art7HttpResponse.GetAppCode(
                        Art7ResponseStatusCode.INVALID_JSON_SCHEMA_FROM_INPUT_CATEGORIES
                    ),
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );

        _dictionary.TryAdd(
            key: Art7ResponseStatusCode.INVALID_FILE_EXTENSION,
            value: (response) => new()
            {
                AppCode = Art7HttpResponse.GetAppCode(Art7ResponseStatusCode.INVALID_FILE_EXTENSION),
                HttpCode = StatusCodes.Status400BadRequest,
            });

        _dictionary.TryAdd(
            key: Art7ResponseStatusCode.INVALID_FILE_FORMAT,
            value: (response) => new()
            {
                AppCode = Art7HttpResponse.GetAppCode(Art7ResponseStatusCode.INVALID_FILE_FORMAT),
                HttpCode = StatusCodes.Status400BadRequest,
            });

        _dictionary.TryAdd(
            key: Art7ResponseStatusCode.FILE_SIZE_IS_EXCEED_THE_LIMIT,
            value: (response) => new()
            {
                AppCode = Art7HttpResponse.GetAppCode(Art7ResponseStatusCode.FILE_SIZE_IS_EXCEED_THE_LIMIT),
                HttpCode = StatusCodes.Status400BadRequest,
            });
    }

    internal static Func<Art7Response, Art7HttpResponse> Resolve(Art7ResponseStatusCode statusCode)
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

        return _dictionary[Art7ResponseStatusCode.FILE_SERVICE_ERROR];
    }
}
