using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM24;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM24.HttpResponse;

public static class SM24HttpResponseManager
{
    private static ConcurrentDictionary<
        SM24ResponseStatusCode,
        Func<SM24Request, SM24Response, SM24HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: SM24ResponseStatusCode.SUCCESS,
            value: (_, response) =>
                new()
                {
                    AppCode = $"SM24.{SM24ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                }
        );

        _dictionary.TryAdd(
            key: SM24ResponseStatusCode.DATABASE_ERROR,
            value: (_, response) =>
                new()
                {
                    AppCode = $"SM24.{SM24ResponseStatusCode.DATABASE_ERROR}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );

        _dictionary.TryAdd(
            key: SM24ResponseStatusCode.FORBIDDEN,
            value: (_, response) =>
                new()
                {
                    AppCode = $"SM24.{SM24ResponseStatusCode.FORBIDDEN}",
                    HttpCode = StatusCodes.Status403Forbidden,
                }
        );

        _dictionary.TryAdd(
            key: SM24ResponseStatusCode.UNAUTHORIZED,
            value: (_, response) =>
                new()
                {
                    AppCode = $"SM24.{SM24ResponseStatusCode.UNAUTHORIZED}",
                    HttpCode = StatusCodes.Status401Unauthorized,
                }
        );
    }

    internal static Func<SM24Request, SM24Response, SM24HttpResponse> Resolve(
        SM24ResponseStatusCode statusCode
    )
    {
        if (Equals(objA: _dictionary, objB: default))
        {
            Init();
        }

        return _dictionary[statusCode];
    }
}
