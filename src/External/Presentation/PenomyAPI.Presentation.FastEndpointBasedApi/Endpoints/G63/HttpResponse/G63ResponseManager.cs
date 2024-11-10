using Microsoft.AspNetCore.Http;
using PenomyAPI.App.G63;
using System;
using System.Collections.Concurrent;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G63.HttpResponse;

public class G63ResponseManager
{
    private static ConcurrentDictionary<
        G63ResponseStatusCode,
        Func<G63Request, G63Response, G63HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: G63ResponseStatusCode.SUCCESS,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G63.{G63ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                }
        );

        _dictionary.TryAdd(
            key: G63ResponseStatusCode.FAILED,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G63.{G63ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status500InternalServerError,
                }
        );
        _dictionary.TryAdd(
            key: G63ResponseStatusCode.INVALID_REQUEST,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G63.{G63ResponseStatusCode.INVALID_REQUEST}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );
    }

    internal static Func<G63Request, G63Response, G63HttpResponse> Resolve(
        G63ResponseStatusCode statusCode
    )
    {
        if (Equals(objA: _dictionary, objB: default))
        {
            Init();
        }

        return _dictionary[statusCode];
    }
}
