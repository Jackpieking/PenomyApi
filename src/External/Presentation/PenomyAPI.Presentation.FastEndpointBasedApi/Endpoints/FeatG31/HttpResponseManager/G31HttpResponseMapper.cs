using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG31;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG31.HttpResponseManager;

internal static class G31HttpResponseMapper
{
    private static ConcurrentDictionary<
        G31ResponseStatusCode,
        Func<G31Request, G31Response, G31HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: G31ResponseStatusCode.SUCCESS,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G31.{G31ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                    Body = response.Body
                }
        );

        _dictionary.TryAdd(
            key: G31ResponseStatusCode.DATABASE_ERROR,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G31.{G31ResponseStatusCode.DATABASE_ERROR}",
                    HttpCode = StatusCodes.Status500InternalServerError,
                }
        );

        _dictionary.TryAdd(
            key: G31ResponseStatusCode.PASSWORD_IS_INCORRECT,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G31.{G31ResponseStatusCode.PASSWORD_IS_INCORRECT}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );

        _dictionary.TryAdd(
            key: G31ResponseStatusCode.TEMPORARY_BANNED,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G31.{G31ResponseStatusCode.TEMPORARY_BANNED}",
                    HttpCode = StatusCodes.Status429TooManyRequests,
                }
        );

        _dictionary.TryAdd(
            key: G31ResponseStatusCode.USER_NOT_FOUND,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G31.{G31ResponseStatusCode.USER_NOT_FOUND}",
                    HttpCode = StatusCodes.Status404NotFound,
                }
        );
    }

    internal static Func<G31Request, G31Response, G31HttpResponse> Resolve(
        G31ResponseStatusCode statusCode
    )
    {
        if (Equals(_dictionary, default))
        {
            Init();
        }

        return _dictionary[statusCode];
    }
}
