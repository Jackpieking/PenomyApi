using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG50;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG50.HttpResponse;

public class G50HttpResponseManager
{
    private static ConcurrentDictionary<
        G50ResponseStatusCode,
        Func<G50Request, G50Response, G50HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary =
            new ConcurrentDictionary<
                G50ResponseStatusCode,
                Func<G50Request, G50Response, G50HttpResponse>
            >();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            G50ResponseStatusCode.SUCCESS,
            (_, response) =>
                new G50HttpResponse
                {
                    AppCode = $"G50.{G50ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK
                }
        );

        _dictionary.TryAdd(
            G50ResponseStatusCode.FAILED,
            (_, response) =>
                new G50HttpResponse
                {
                    AppCode = $"G50.{G50ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status500InternalServerError
                }
        );
        _dictionary.TryAdd(
            G50ResponseStatusCode.NOT_FOUND,
            (_, response) =>
                new G50HttpResponse
                {
                    AppCode = $"G50.{G50ResponseStatusCode.NOT_FOUND}",
                    HttpCode = StatusCodes.Status404NotFound
                }
        );
        _dictionary.TryAdd(
            G50ResponseStatusCode.UNAUTHORIZED,
            (_, response) =>
                new G50HttpResponse
                {
                    AppCode = $"G50.{G50ResponseStatusCode.UNAUTHORIZED}",
                    HttpCode = StatusCodes.Status401Unauthorized
                }
        );
    }

    internal static Func<G50Request, G50Response, G50HttpResponse> Resolve(
        G50ResponseStatusCode statusCode
    )
    {
        if (Equals(_dictionary, default))
            Init();

        return _dictionary[statusCode];
    }
}
