using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM46;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM46.HttpResponse;

public static class SM46HttpResponseManager
{
    private static ConcurrentDictionary<
        SM46ResponseStatusCode,
        Func<SM46Response, SM46HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: SM46ResponseStatusCode.SUCCESS,
            value: (response) =>
                new()
                {
                    AppCode = $"SM46.{SM46ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                }
        );

        _dictionary.TryAdd(
            key: SM46ResponseStatusCode.FAILED,
            value: (response) =>
                new()
                {
                    AppCode = $"SM46.{SM46ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );

        _dictionary.TryAdd(
            key: SM46ResponseStatusCode.FAILED,
            value: (response) =>
                new()
                {
                    AppCode = $"SM46.{SM46ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status401Unauthorized,
                }
        );

        _dictionary.TryAdd(
            key: SM46ResponseStatusCode.FORBIDDEN,
            value: (response) =>
                new()
                {
                    AppCode = $"SM46.{SM46ResponseStatusCode.FORBIDDEN}",
                    HttpCode = StatusCodes.Status403Forbidden,
                }
        );
    }

    internal static Func<SM46Response, SM46HttpResponse> Resolve(SM46ResponseStatusCode statusCode)
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
