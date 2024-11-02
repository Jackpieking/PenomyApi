using Microsoft.AspNetCore.Http;
using PenomyAPI.App.G61;
using System;
using System.Collections.Concurrent;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G61.HttpResponse;

public class G61ResponseManager
{
    private static ConcurrentDictionary<
        G61ResponseStatusCode,
        Func<G61Request, G61Response, G61HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: G61ResponseStatusCode.SUCCESS,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G61.{G61ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                }
        );

        _dictionary.TryAdd(
            key: G61ResponseStatusCode.FAILED,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G61.{G61ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status500InternalServerError,
                }
        );
        _dictionary.TryAdd(
            key: G61ResponseStatusCode.INVALID_REQUEST,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G61.{G61ResponseStatusCode.INVALID_REQUEST}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );
    }

    internal static Func<G61Request, G61Response, G61HttpResponse> Resolve(
        G61ResponseStatusCode statusCode
    )
    {
        if (Equals(objA: _dictionary, objB: default))
        {
            Init();
        }

        return _dictionary[statusCode];
    }
}
