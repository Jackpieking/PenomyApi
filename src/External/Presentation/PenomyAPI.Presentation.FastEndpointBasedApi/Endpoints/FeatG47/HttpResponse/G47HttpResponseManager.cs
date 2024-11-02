using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG47;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG47.HttpResponse;

public class G47HttpResponseManager
{
    private static ConcurrentDictionary<
        G47ResponseStatusCode,
        Func<G47Request, G47Response, G47HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new ConcurrentDictionary<G47ResponseStatusCode, Func<G47Request, G47Response, G47HttpResponse>>();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            G47ResponseStatusCode.SUCCESS,
            (_, response) =>
                new G47HttpResponse
                {
                    AppCode = $"G47.{G47ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK
                }
        );

        _dictionary.TryAdd(
            G47ResponseStatusCode.FAILED,
            (_, response) =>
                new G47HttpResponse
                {
                    AppCode = $"G47.{G47ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status500InternalServerError
                }
        );
        _dictionary.TryAdd(
            G47ResponseStatusCode.INVALID_REQUEST,
            (_, response) =>
                new G47HttpResponse
                {
                    AppCode = $"G47.{G47ResponseStatusCode.INVALID_REQUEST}",
                    HttpCode = StatusCodes.Status400BadRequest
                }
        );
        _dictionary.TryAdd(
            G47ResponseStatusCode.NOT_FOUND,
            (_, response) =>
                new G47HttpResponse
                {
                    AppCode = $"G47.{G47ResponseStatusCode.NOT_FOUND}",
                    HttpCode = StatusCodes.Status404NotFound
                }
        );
        _dictionary.TryAdd(
            G47ResponseStatusCode.UNAUTHORIZED,
            (_, response) =>
                new G47HttpResponse
                {
                    AppCode = $"G47.{G47ResponseStatusCode.UNAUTHORIZED}",
                    HttpCode = StatusCodes.Status401Unauthorized
                }
        );
    }

    internal static Func<G47Request, G47Response, G47HttpResponse> Resolve(
        G47ResponseStatusCode statusCode
    )
    {
        if (Equals(_dictionary, default)) Init();

        return _dictionary[statusCode];
    }
}
