using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM45;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM45.HttpResponse;

public static class SM45HttpResponseManager
{
    private static ConcurrentDictionary<
        SM45ResponseStatusCode,
        Func<SM45Response, SM45HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: SM45ResponseStatusCode.SUCCESS,
            value: (response) =>
                new()
                {
                    AppCode = $"SM45.{SM45ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                }
        );

        _dictionary.TryAdd(
            key: SM45ResponseStatusCode.FAILED,
            value: (response) =>
                new()
                {
                    AppCode = $"SM45.{SM45ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );

        _dictionary.TryAdd(
            key: SM45ResponseStatusCode.FAILED,
            value: (response) =>
                new()
                {
                    AppCode = $"SM45.{SM45ResponseStatusCode.UNAUTHORIZED}",
                    HttpCode = StatusCodes.Status401Unauthorized,
                }
        );
    }

    internal static Func<SM45Response, SM45HttpResponse> Resolve(SM45ResponseStatusCode statusCode)
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
