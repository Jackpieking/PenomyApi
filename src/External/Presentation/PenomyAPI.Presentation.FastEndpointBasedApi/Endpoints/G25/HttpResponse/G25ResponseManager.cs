using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.G25;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25.HttpResponse;

public class G25ResponseManager
{
    private static ConcurrentDictionary<
        G25ResponseStatusCode,
        Func<G25Request, G25Response, G25HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: G25ResponseStatusCode.SUCCESS,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G25.{G25ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                }
        );

        _dictionary.TryAdd(
            key: G25ResponseStatusCode.EMPTY,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G25.{G25ResponseStatusCode.EMPTY}",
                    HttpCode = StatusCodes.Status404NotFound,
                }
        );
    }

    internal static Func<G25Request, G25Response, G25HttpResponse> Resolve(
        G25ResponseStatusCode statusCode
    )
    {
        if (Equals(objA: _dictionary, objB: default))
        {
            Init();
        }

        return _dictionary[statusCode];
    }
}
