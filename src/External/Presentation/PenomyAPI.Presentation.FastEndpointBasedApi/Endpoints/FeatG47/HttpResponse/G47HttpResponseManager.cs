namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG47.HttpResponse;

using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG47;
using System;
using System.Collections.Concurrent;

public class G47HttpResponseManager
{
    private static ConcurrentDictionary<
        G47ResponseStatusCode,
        Func<G47Request, G47Response, G47HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: G47ResponseStatusCode.SUCCESS,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G47.{G47ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                }
        );

        _dictionary.TryAdd(
            key: G47ResponseStatusCode.FAILED,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G47.{G47ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status500InternalServerError,
                }
        );
        _dictionary.TryAdd(
            key: G47ResponseStatusCode.INVALID_REQUEST,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G47.{G47ResponseStatusCode.INVALID_REQUEST}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );
        _dictionary.TryAdd(
            key: G47ResponseStatusCode.NOT_FOUND,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G47.{G47ResponseStatusCode.NOT_FOUND}",
                    HttpCode = StatusCodes.Status404NotFound,
                }
        );
    }

    internal static Func<G47Request, G47Response, G47HttpResponse> Resolve(
        G47ResponseStatusCode statusCode
    )
    {
        if (Equals(objA: _dictionary, objB: default))
        {
            Init();
        }

        return _dictionary[statusCode];
    }
}
