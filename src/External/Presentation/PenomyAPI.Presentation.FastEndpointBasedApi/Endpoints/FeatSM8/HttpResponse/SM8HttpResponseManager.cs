﻿using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM8;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM8.HttpResponse;

public static class SM8HttpResponseManager
{
    private static ConcurrentDictionary<
        SM8ResponseStatusCode,
        Func<SM8Response, SM8HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: SM8ResponseStatusCode.SUCCESS,
            value: (response) =>
                new()
                {
                    AppCode = $"SM8.{SM8ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                }
        );

        _dictionary.TryAdd(
            key: SM8ResponseStatusCode.FAILED,
            value: (response) =>
                new()
                {
                    AppCode = $"SM8.{SM8ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );

        _dictionary.TryAdd(
            key: SM8ResponseStatusCode.FORBIDDEN,
            value: (response) =>
                new()
                {
                    AppCode = $"SM8.{SM8ResponseStatusCode.FORBIDDEN}",
                    HttpCode = StatusCodes.Status403Forbidden,
                }
        );

        _dictionary.TryAdd(
            key: SM8ResponseStatusCode.UN_AUTHORIZED,
            value: (response) =>
                new()
                {
                    AppCode = $"SM8.{SM8ResponseStatusCode.UN_AUTHORIZED}",
                    HttpCode = StatusCodes.Status401Unauthorized,
                }
        );

        _dictionary.TryAdd(
            key: SM8ResponseStatusCode.INVALID_FILE_EXTENSION,
            value: (response) =>
                new()
                {
                    AppCode = $"SM8.{SM8ResponseStatusCode.INVALID_FILE_EXTENSION}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );

        _dictionary.TryAdd(
            key: SM8ResponseStatusCode.INVALID_FILE_FORMAT,
            value: (response) =>
                new()
                {
                    AppCode = $"SM8.{SM8ResponseStatusCode.INVALID_FILE_FORMAT}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );
        _dictionary.TryAdd(
            key: SM8ResponseStatusCode.MAXIMUM_IMAGE_FILE_SIZE,
            value: (response) =>
                new()
                {
                    AppCode = $"SM8.{SM8ResponseStatusCode.MAXIMUM_IMAGE_FILE_SIZE}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );
    }

    internal static Func<SM8Response, SM8HttpResponse> Resolve(
        SM8ResponseStatusCode statusCode
    )
    {
        if (Equals(objA: _dictionary, objB: default))
        {
            Init();
        }

        if (_dictionary.TryGetValue(statusCode, out var response))
        {
            return response;
        }

        return _dictionary[statusCode];
    }
}