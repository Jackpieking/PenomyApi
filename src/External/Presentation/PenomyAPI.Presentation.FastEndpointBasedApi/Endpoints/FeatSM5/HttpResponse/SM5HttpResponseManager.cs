using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM5;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM5.HttpResponse;

public static class SM5HttpResponseManager
{
    private static ConcurrentDictionary<
        SM5ResponseStatusCode,
        Func<SM5Response, SM5HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: SM5ResponseStatusCode.SUCCESS,
            value: (response) =>
                new()
                {
                    AppCode = $"SM5.{SM5ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                }
        );

        _dictionary.TryAdd(
            key: SM5ResponseStatusCode.FAILED,
            value: (response) =>
                new()
                {
                    AppCode = $"SM5.{SM5ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );

        _dictionary.TryAdd(
            key: SM5ResponseStatusCode.UN_AUTHORIZED,
            value: (response) =>
                new()
                {
                    AppCode = $"SM5.{SM5ResponseStatusCode.UN_AUTHORIZED}",
                    HttpCode = StatusCodes.Status401Unauthorized,
                }
        );

        _dictionary.TryAdd(
            key: SM5ResponseStatusCode.DATABASE_ERROR,
            value: (response) =>
                new()
                {
                    AppCode = $"SM5.{SM5ResponseStatusCode.DATABASE_ERROR}",
                    HttpCode = StatusCodes.Status404NotFound,
                }
        );

    }

    internal static Func<SM5Response, SM5HttpResponse> Resolve(
        SM5ResponseStatusCode statusCode
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
