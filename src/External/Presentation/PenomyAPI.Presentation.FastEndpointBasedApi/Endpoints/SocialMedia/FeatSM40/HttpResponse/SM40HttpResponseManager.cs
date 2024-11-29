using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM40;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM40.HttpResponse;

public static class SM40HttpResponseManager
{
    private static ConcurrentDictionary<
        SM40ResponseStatusCode,
        Func<SM40Response, SM40HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: SM40ResponseStatusCode.SUCCESS,
            value: (response) =>
                new()
                {
                    AppCode = $"SM40.{SM40ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                }
        );

        _dictionary.TryAdd(
            key: SM40ResponseStatusCode.FAILED,
            value: (response) =>
                new()
                {
                    AppCode = $"SM40.{SM40ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );

        _dictionary.TryAdd(
            key: SM40ResponseStatusCode.UNAUTHORIZED,
            value: (response) =>
                new()
                {
                    AppCode = $"SM40.{SM40ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status401Unauthorized,
                }
        );
        
        _dictionary.TryAdd(
            key: SM40ResponseStatusCode.FOBIDDEN,
            value: (response) =>
                new()
                {
                    AppCode = $"SM40.{SM40ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status403Forbidden,
                }
        );
    }

    internal static Func<SM40Response, SM40HttpResponse> Resolve(SM40ResponseStatusCode statusCode)
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
