using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM42;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM42.HttpResponse;

public static class SM42HttpResponseManager
{
    private static ConcurrentDictionary<
        SM42ResponseStatusCode,
        Func<SM42Response, SM42HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: SM42ResponseStatusCode.SUCCESS,
            value: (response) =>
                new()
                {
                    AppCode = $"SM42.{SM42ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                }
        );

        _dictionary.TryAdd(
            key: SM42ResponseStatusCode.FAILED,
            value: (response) =>
                new()
                {
                    AppCode = $"SM42.{SM42ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );
    }

    internal static Func<SM42Response, SM42HttpResponse> Resolve(SM42ResponseStatusCode statusCode)
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
