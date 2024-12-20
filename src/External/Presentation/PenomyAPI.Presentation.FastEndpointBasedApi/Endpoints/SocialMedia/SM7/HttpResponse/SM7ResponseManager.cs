using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM7;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM7.HttpResponse;

public class SM7ResponseManager
{
    private static ConcurrentDictionary<
        SM7ResponseStatusCode,
        Func<SM7Request, SM7Response, SM7HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: SM7ResponseStatusCode.SUCCESS,
            value: (_, response) =>
                new()
                {
                    AppCode = $"SM7.{SM7ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                }
        );

        _dictionary.TryAdd(
            key: SM7ResponseStatusCode.FAILED,
            value: (_, response) =>
                new()
                {
                    AppCode = $"SM7.{SM7ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status500InternalServerError,
                }
        );
        _dictionary.TryAdd(
            key: SM7ResponseStatusCode.INVALID_REQUEST,
            value: (_, response) =>
                new()
                {
                    AppCode = $"SM7.{SM7ResponseStatusCode.INVALID_REQUEST}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );
    }

    internal static Func<SM7Request, SM7Response, SM7HttpResponse> Resolve(
        SM7ResponseStatusCode statusCode
    )
    {
        if (Equals(objA: _dictionary, objB: default))
        {
            Init();
        }

        return _dictionary[statusCode];
    }
}
