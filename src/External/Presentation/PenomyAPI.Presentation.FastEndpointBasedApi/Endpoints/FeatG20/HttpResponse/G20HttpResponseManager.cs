using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG20;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G20.HttpResponse;

public static class G20HttpResponseManager
{
    private static ConcurrentDictionary<
        G20ResponseStatusCode,
        Func<G20Request, G20Response, G20HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: G20ResponseStatusCode.SUCCESS,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G20.{G20ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                }
        );

        _dictionary.TryAdd(
            key: G20ResponseStatusCode.DATABASE_ERROR,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G20.{G20ResponseStatusCode.DATABASE_ERROR}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );
    }

    internal static Func<G20Request, G20Response, G20HttpResponse> Resolve(
        G20ResponseStatusCode statusCode
    )
    {
        if (Equals(objA: _dictionary, objB: default))
        {
            Init();
        }

        return _dictionary[statusCode];
    }
}
