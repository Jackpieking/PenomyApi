using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG53;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG53.HttpResponse;

public static class G53HttpResponseManager
{
    private static ConcurrentDictionary<
        G53ResponseStatusCode,
        Func<G53Request, G53Response, G53HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: G53ResponseStatusCode.SUCCESS,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G53.{G53ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                }
        );

        _dictionary.TryAdd(
            key: G53ResponseStatusCode.DATABASE_ERROR,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G53.{G53ResponseStatusCode.DATABASE_ERROR}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );

        _dictionary.TryAdd(
            key: G53ResponseStatusCode.FORBIDDEN,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G53.{G53ResponseStatusCode.FORBIDDEN}",
                    HttpCode = StatusCodes.Status403Forbidden,
                }
        );

        _dictionary.TryAdd(
            key: G53ResponseStatusCode.UN_AUTHORIZED,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G53.{G53ResponseStatusCode.UN_AUTHORIZED}",
                    HttpCode = StatusCodes.Status401Unauthorized,
                }
        );
    }

    internal static Func<G53Request, G53Response, G53HttpResponse> Resolve(
        G53ResponseStatusCode statusCode
    )
    {
        if (Equals(objA: _dictionary, objB: default))
        {
            Init();
        }

        return _dictionary[statusCode];
    }
}
