using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM43;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM43.HttpResponse;

public static class SM43HttpResponseManager
{
    private static ConcurrentDictionary<
        SM43ResponseStatusCode,
        Func<SM43Response, SM43HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: SM43ResponseStatusCode.SUCCESS,
            value: (response) =>
                new()
                {
                    AppCode = $"SM43.{SM43ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                }
        );

        _dictionary.TryAdd(
            key: SM43ResponseStatusCode.FAILED,
            value: (response) =>
                new()
                {
                    AppCode = $"SM43.{SM43ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );
    }

    internal static Func<SM43Response, SM43HttpResponse> Resolve(SM43ResponseStatusCode statusCode)
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
