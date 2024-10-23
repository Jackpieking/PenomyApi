using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG46;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G46.HttpResponse;
using System;
using System.Collections.Concurrent;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG46.HttpResponse;

public class G46HttpResponseManager
{
    private static ConcurrentDictionary<
        G46ResponseStatusCode,
        Func<G46Request, G46Response, G46HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: G46ResponseStatusCode.SUCCESS,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G46.{G46ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                }
        );

        _dictionary.TryAdd(
            key: G46ResponseStatusCode.FAILED,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G46.{G46ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status500InternalServerError,
                }
        );
        _dictionary.TryAdd(
            key: G46ResponseStatusCode.INVALID_REQUEST,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G46.{G46ResponseStatusCode.INVALID_REQUEST}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );
        _dictionary.TryAdd(
            key: G46ResponseStatusCode.NOT_FOUND,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G46.{G46ResponseStatusCode.NOT_FOUND}",
                    HttpCode = StatusCodes.Status404NotFound,
                }
        );
        _dictionary.TryAdd(
            key: G46ResponseStatusCode.EXISTED,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G46.{G46ResponseStatusCode.EXISTED}",
                    HttpCode = StatusCodes.Status409Conflict,
                }
        );
    }

    internal static Func<G46Request, G46Response, G46HttpResponse> Resolve(
        G46ResponseStatusCode statusCode
    )
    {
        if (Equals(objA: _dictionary, objB: default))
        {
            Init();
        }

        return _dictionary[statusCode];
    }
}
