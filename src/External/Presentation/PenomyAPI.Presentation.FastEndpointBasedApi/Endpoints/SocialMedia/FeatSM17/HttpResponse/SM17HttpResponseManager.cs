using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM17;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM17.HttpResponse;

public static class SM17HttpResponseManager
{
    private static ConcurrentDictionary<
        SM17ResponseStatusCode,
        Func<SM17Response, SM17HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: SM17ResponseStatusCode.SUCCESS,
            value: (response) =>
                new()
                {
                    AppCode = $"SM17.{SM17ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                }
        );

        _dictionary.TryAdd(
            key: SM17ResponseStatusCode.FAILED,
            value: (response) =>
                new()
                {
                    AppCode = $"SM17.{SM17ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );

        _dictionary.TryAdd(
            key: SM17ResponseStatusCode.UNAUTHORIZED,
            value: (response) =>
                new()
                {
                    AppCode = $"SM17.{SM17ResponseStatusCode.UNAUTHORIZED}",
                    HttpCode = StatusCodes.Status401Unauthorized,
                }
        );

        _dictionary.TryAdd(
            key: SM17ResponseStatusCode.DATABASE_ERROR,
            value: (response) =>
                new()
                {
                    AppCode = $"SM17.{SM17ResponseStatusCode.DATABASE_ERROR}",
                    HttpCode = StatusCodes.Status404NotFound,
                }
        );
    }

    internal static Func<SM17Response, SM17HttpResponse> Resolve(SM17ResponseStatusCode statusCode)
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
