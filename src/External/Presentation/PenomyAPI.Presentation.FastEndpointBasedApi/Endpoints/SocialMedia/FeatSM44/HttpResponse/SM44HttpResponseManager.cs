using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM44;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM44.HttpResponse;

public static class SM44HttpResponseManager
{
    private static ConcurrentDictionary<
        SM44ResponseStatusCode,
        Func<SM44Response, SM44HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: SM44ResponseStatusCode.SUCCESS,
            value: (response) =>
                new()
                {
                    AppCode = $"SM44.{SM44ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                }
        );

        _dictionary.TryAdd(
            key: SM44ResponseStatusCode.FAILED,
            value: (response) =>
                new()
                {
                    AppCode = $"SM44.{SM44ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );
    }

    internal static Func<SM44Response, SM44HttpResponse> Resolve(SM44ResponseStatusCode statusCode)
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
