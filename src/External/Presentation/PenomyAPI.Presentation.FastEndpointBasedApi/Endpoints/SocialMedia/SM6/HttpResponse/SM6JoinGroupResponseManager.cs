using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM6;
using System;
using System.Collections.Concurrent;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.SM6.HttpResponse;

public class SM6JoinGroupResponseManager
{
    private static ConcurrentDictionary<
        SM6ResponseStatusCode,
        Func<SM6Request, SM6Response, SM6JoinGroupHttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: SM6ResponseStatusCode.SUCCESS,
            value: (_, response) =>
                new()
                {
                    AppCode = $"SM6.{SM6ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                }
        );

        _dictionary.TryAdd(
            key: SM6ResponseStatusCode.FAILED,
            value: (_, response) =>
                new()
                {
                    AppCode = $"SM6.{SM6ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status500InternalServerError,
                }
        );
        _dictionary.TryAdd(
            key: SM6ResponseStatusCode.INVALID_REQUEST,
            value: (_, response) =>
                new()
                {
                    AppCode = $"SM6.{SM6ResponseStatusCode.INVALID_REQUEST}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );
    }

    internal static Func<SM6Request, SM6Response, SM6JoinGroupHttpResponse> Resolve(
        SM6ResponseStatusCode statusCode
    )
    {
        if (Equals(objA: _dictionary, objB: default))
        {
            Init();
        }

        return _dictionary[statusCode];
    }
}
