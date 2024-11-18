using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM1;
using System;
using System.Collections.Concurrent;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM1.HttpResponse;

public class SM1ResponseManager
{
    private static ConcurrentDictionary<
        SM1ResponseStatusCode,
        Func<SM1Request, SM1Response, SM1HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: SM1ResponseStatusCode.SUCCESS,
            value: (_, response) =>
                new()
                {
                    AppCode = $"SM1.{SM1ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                }
        );

        _dictionary.TryAdd(
            key: SM1ResponseStatusCode.FAILED,
            value: (_, response) =>
                new()
                {
                    AppCode = $"SM1.{SM1ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status500InternalServerError,
                }
        );
        _dictionary.TryAdd(
            key: SM1ResponseStatusCode.INVALID_REQUEST,
            value: (_, response) =>
                new()
                {
                    AppCode = $"SM1.{SM1ResponseStatusCode.INVALID_REQUEST}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );
    }

    internal static Func<SM1Request, SM1Response, SM1HttpResponse> Resolve(
        SM1ResponseStatusCode statusCode
    )
    {
        if (Equals(objA: _dictionary, objB: default))
        {
            Init();
        }

        return _dictionary[statusCode];
    }
}
