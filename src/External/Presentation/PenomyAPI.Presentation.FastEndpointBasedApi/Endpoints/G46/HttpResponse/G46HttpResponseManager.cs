using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG46;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G46.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG46.HttpResponse;

public class G46HttpResponseManager
{
    private static ConcurrentDictionary<
        G46ResponseStatusCode,
        Func<G46Request, G46Response, G46HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new ConcurrentDictionary<G46ResponseStatusCode, Func<G46Request, G46Response, G46HttpResponse>>();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            G46ResponseStatusCode.SUCCESS,
            (_, response) =>
                new G46HttpResponse
                {
                    AppCode = $"G46.{G46ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK
                }
        );

        _dictionary.TryAdd(
            G46ResponseStatusCode.FAILED,
            (_, response) =>
                new G46HttpResponse
                {
                    AppCode = $"G46.{G46ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status500InternalServerError
                }
        );
        _dictionary.TryAdd(
            G46ResponseStatusCode.INVALID_REQUEST,
            (_, response) =>
                new G46HttpResponse
                {
                    AppCode = $"G46.{G46ResponseStatusCode.INVALID_REQUEST}",
                    HttpCode = StatusCodes.Status400BadRequest
                }
        );
        _dictionary.TryAdd(
            G46ResponseStatusCode.NOT_FOUND,
            (_, response) =>
                new G46HttpResponse
                {
                    AppCode = $"G46.{G46ResponseStatusCode.NOT_FOUND}",
                    HttpCode = StatusCodes.Status404NotFound
                }
        );
        _dictionary.TryAdd(
            G46ResponseStatusCode.UNAUTHORIZED,
            (_, response) =>
                new G46HttpResponse
                {
                    AppCode = $"G46.{G46ResponseStatusCode.UNAUTHORIZED}",
                    HttpCode = StatusCodes.Status401Unauthorized
                }
        );
        _dictionary.TryAdd(
            G46ResponseStatusCode.EXISTED,
            (_, response) =>
                new G46HttpResponse
                {
                    AppCode = $"G46.{G46ResponseStatusCode.EXISTED}",
                    HttpCode = StatusCodes.Status409Conflict
                }
        );
    }

    internal static Func<G46Request, G46Response, G46HttpResponse> Resolve(
        G46ResponseStatusCode statusCode
    )
    {
        if (Equals(_dictionary, default)) Init();

        return _dictionary[statusCode];
    }
}
