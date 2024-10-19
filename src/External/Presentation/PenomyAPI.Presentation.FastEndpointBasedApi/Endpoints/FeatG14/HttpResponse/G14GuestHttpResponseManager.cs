using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG14;
using PenomyAPI.App.FeatG14.OtherHandler;
using System;
using System.Collections.Concurrent;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG14.HttpResponse;

public class G14GuestHttpResponseManager
{
    private static ConcurrentDictionary<
    G14ResponseStatusCode,
    Func<G14GuestRequest, G14GuestResponse, G14HttpResponse>
> _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: G14ResponseStatusCode.SUCCESS,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G14Guest.{G14ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                }
        );

        _dictionary.TryAdd(
            key: G14ResponseStatusCode.FAILED,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G14Guest.{G14ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status500InternalServerError,
                }
        );
        _dictionary.TryAdd(
            key: G14ResponseStatusCode.INVALID_REQUEST,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G14Guest.{G14ResponseStatusCode.INVALID_REQUEST}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );
        _dictionary.TryAdd(
            key: G14ResponseStatusCode.NOT_FOUND,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G14Guest.{G14ResponseStatusCode.NOT_FOUND}",
                    HttpCode = StatusCodes.Status404NotFound,
                }
        );
    }

    internal static Func<G14GuestRequest, G14GuestResponse, G14HttpResponse> Resolve(
        G14ResponseStatusCode statusCode
    )
    {
        if (Equals(objA: _dictionary, objB: default))
        {
            Init();
        }

        return _dictionary[statusCode];
    }
}
