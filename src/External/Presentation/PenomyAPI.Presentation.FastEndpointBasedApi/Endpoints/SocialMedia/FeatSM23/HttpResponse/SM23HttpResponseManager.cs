using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM23;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM23.HttpResponse;

public static class SM23HttpResponseManager
{
    private static ConcurrentDictionary<
        SM23ResponseStatusCode,
        Func<SM23Response, SM23HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: SM23ResponseStatusCode.SUCCESS,
            value: (response) =>
                new()
                {
                    AppCode = $"SM23.{SM23ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                }
        );

        _dictionary.TryAdd(
            key: SM23ResponseStatusCode.FAILED,
            value: (response) =>
                new()
                {
                    AppCode = $"SM23.{SM23ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );

        _dictionary.TryAdd(
            key: SM23ResponseStatusCode.UNAUTHORIZED,
            value: (response) =>
                new()
                {
                    AppCode = $"SM23.{SM23ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status401Unauthorized,
                }
        );
        
    }

    internal static Func<SM23Response, SM23HttpResponse> Resolve(SM23ResponseStatusCode statusCode)
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
