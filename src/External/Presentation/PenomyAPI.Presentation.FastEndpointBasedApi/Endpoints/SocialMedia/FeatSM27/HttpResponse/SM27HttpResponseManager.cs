using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM27;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM27.HttpResponse;

public static class SM27HttpResponseManager
{
    private static ConcurrentDictionary<
        SM27ResponseStatusCode,
        Func<SM27Request, SM27Response, SM27HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: SM27ResponseStatusCode.SUCCESS,
            value: (_, response) =>
                new()
                {
                    AppCode = $"SM27.{SM27ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                }
        );

        _dictionary.TryAdd(
            key: SM27ResponseStatusCode.DATABASE_ERROR,
            value: (_, response) =>
                new()
                {
                    AppCode = $"SM27.{SM27ResponseStatusCode.DATABASE_ERROR}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );

        _dictionary.TryAdd(
            key: SM27ResponseStatusCode.FORBIDDEN,
            value: (_, response) =>
                new()
                {
                    AppCode = $"SM27.{SM27ResponseStatusCode.FORBIDDEN}",
                    HttpCode = StatusCodes.Status403Forbidden,
                }
        );

        _dictionary.TryAdd(
            key: SM27ResponseStatusCode.UNAUTHORIZED,
            value: (_, response) =>
                new()
                {
                    AppCode = $"SM27.{SM27ResponseStatusCode.UNAUTHORIZED}",
                    HttpCode = StatusCodes.Status401Unauthorized,
                }
        );
    }

    internal static Func<SM27Request, SM27Response, SM27HttpResponse> Resolve(
        SM27ResponseStatusCode statusCode
    )
    {
        if (Equals(objA: _dictionary, objB: default))
        {
            Init();
        }

        return _dictionary[statusCode];
    }
}
