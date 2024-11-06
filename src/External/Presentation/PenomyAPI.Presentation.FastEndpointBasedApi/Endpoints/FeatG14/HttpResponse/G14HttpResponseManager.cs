using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG14;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG14.HttpResponse;

public class G14HttpResponseManager
{
    private static ConcurrentDictionary<
        G14ResponseStatusCode,
        Func<G14Request, G14Response, G14HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new ConcurrentDictionary<G14ResponseStatusCode, Func<G14Request, G14Response, G14HttpResponse>>();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            G14ResponseStatusCode.SUCCESS,
            (_, response) =>
                new G14HttpResponse
                {
                    AppCode = $"G14.{G14ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK
                }
        );

        _dictionary.TryAdd(
            G14ResponseStatusCode.FAILED,
            (_, response) =>
                new G14HttpResponse
                {
                    AppCode = $"G14.{G14ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status500InternalServerError
                }
        );
        _dictionary.TryAdd(
            G14ResponseStatusCode.INVALID_REQUEST,
            (_, response) =>
                new G14HttpResponse
                {
                    AppCode = $"G14.{G14ResponseStatusCode.INVALID_REQUEST}",
                    HttpCode = StatusCodes.Status400BadRequest
                }
        );
        _dictionary.TryAdd(
            G14ResponseStatusCode.INVALID_REQUEST,
            (_, response) =>
                new G14HttpResponse
                {
                    AppCode = $"G14.{G14ResponseStatusCode.UNAUTHORIZED}",
                    HttpCode = StatusCodes.Status400BadRequest
                }
        );
        _dictionary.TryAdd(
            G14ResponseStatusCode.NOT_FOUND,
            (_, response) =>
                new G14HttpResponse
                {
                    AppCode = $"G14.{G14ResponseStatusCode.NOT_FOUND}",
                    HttpCode = StatusCodes.Status404NotFound
                }
        );
    }

    internal static Func<G14Request, G14Response, G14HttpResponse> Resolve(
        G14ResponseStatusCode statusCode
    )
    {
        if (Equals(_dictionary, default)) Init();

        return _dictionary[statusCode];
    }
}
