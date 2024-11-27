using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM41;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM41.HttpResponse;

public static class SM41HttpResponseManager
{
    private static ConcurrentDictionary<
        SM41ResponseStatusCode,
        Func<SM41Response, SM41HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: SM41ResponseStatusCode.SUCCESS,
            value: (response) =>
                new()
                {
                    AppCode = $"SM41.{SM41ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                }
        );

        _dictionary.TryAdd(
            key: SM41ResponseStatusCode.FAILED,
            value: (response) =>
                new()
                {
                    AppCode = $"SM41.{SM41ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );
    }

    internal static Func<SM41Response, SM41HttpResponse> Resolve(SM41ResponseStatusCode statusCode)
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
