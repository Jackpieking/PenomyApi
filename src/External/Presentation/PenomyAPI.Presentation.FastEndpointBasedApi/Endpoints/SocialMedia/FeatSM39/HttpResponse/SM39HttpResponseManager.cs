using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM39;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM39.HttpResponse;

public static class SM39HttpResponseManager
{
    private static ConcurrentDictionary<
        SM39ResponseStatusCode,
        Func<SM39Response, SM39HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: SM39ResponseStatusCode.SUCCESS,
            value: (response) =>
                new()
                {
                    AppCode = $"SM39.{SM39ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                }
        );

        _dictionary.TryAdd(
            key: SM39ResponseStatusCode.FAILED,
            value: (response) =>
                new()
                {
                    AppCode = $"SM39.{SM39ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );
    }

    internal static Func<SM39Response, SM39HttpResponse> Resolve(SM39ResponseStatusCode statusCode)
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
