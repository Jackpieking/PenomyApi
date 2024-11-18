using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM9;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM9.HttpResponse;

public static class SM9HttpResponseManager
{
    private static ConcurrentDictionary<
        SM9ResponseStatusCode,
        Func<SM9Response, SM9HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: SM9ResponseStatusCode.SUCCESS,
            value: (response) =>
                new()
                {
                    AppCode = $"SM9.{SM9ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                }
        );

        _dictionary.TryAdd(
            key: SM9ResponseStatusCode.FAILED,
            value: (response) =>
                new()
                {
                    AppCode = $"SM9.{SM9ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );

        _dictionary.TryAdd(
            key: SM9ResponseStatusCode.UN_AUTHORIZED,
            value: (response) =>
                new()
                {
                    AppCode = $"SM9.{SM9ResponseStatusCode.UN_AUTHORIZED}",
                    HttpCode = StatusCodes.Status401Unauthorized,
                }
        );

        _dictionary.TryAdd(
            key: SM9ResponseStatusCode.DATABASE_ERROR,
            value: (response) =>
                new()
                {
                    AppCode = $"SM9.{SM9ResponseStatusCode.DATABASE_ERROR}",
                    HttpCode = StatusCodes.Status404NotFound,
                }
        );

    }

    internal static Func<SM9Response, SM9HttpResponse> Resolve(
        SM9ResponseStatusCode statusCode
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
