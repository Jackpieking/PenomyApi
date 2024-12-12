using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM38.CoverImage;
using PenomyAPI.App.SM38;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM38Image.HttpResponse;

public static class SM38ImageHttpResponseManager
{
    private static ConcurrentDictionary<
        SM38ResponseStatusCode,
        Func<SM38ImageResponse, SM38ImageHttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: SM38ResponseStatusCode.SUCCESS,
            value: (response) =>
                new()
                {
                    AppCode = $"SM38.{SM38ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                }
        );

        _dictionary.TryAdd(
            key: SM38ResponseStatusCode.FAILED,
            value: (response) =>
                new()
                {
                    AppCode = $"SM38.{SM38ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );

        _dictionary.TryAdd(
            key: SM38ResponseStatusCode.UN_AUTHORIZED,
            value: (response) =>
                new()
                {
                    AppCode = $"SM38.{SM38ResponseStatusCode.UN_AUTHORIZED}",
                    HttpCode = StatusCodes.Status401Unauthorized,
                }
        );

        _dictionary.TryAdd(
            key: SM38ResponseStatusCode.INVALID_FILE_EXTENSION,
            value: (response) =>
                new()
                {
                    AppCode = $"SM38.{SM38ResponseStatusCode.INVALID_FILE_EXTENSION}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );

        _dictionary.TryAdd(
            key: SM38ResponseStatusCode.INVALID_FILE_FORMAT,
            value: (response) =>
                new()
                {
                    AppCode = $"SM38.{SM38ResponseStatusCode.INVALID_FILE_FORMAT}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );
        _dictionary.TryAdd(
            key: SM38ResponseStatusCode.MAXIMUM_IMAGE_FILE_SIZE,
            value: (response) =>
                new()
                {
                    AppCode = $"SM38.{SM38ResponseStatusCode.MAXIMUM_IMAGE_FILE_SIZE}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );
        _dictionary.TryAdd(
            key: SM38ResponseStatusCode.DATABSE_ERROR,
            value: (response) =>
                new()
                {
                    AppCode = $"SM38.{SM38ResponseStatusCode.DATABSE_ERROR}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );
    }

    internal static Func<SM38ImageResponse, SM38ImageHttpResponse> Resolve(
        SM38ResponseStatusCode statusCode
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
