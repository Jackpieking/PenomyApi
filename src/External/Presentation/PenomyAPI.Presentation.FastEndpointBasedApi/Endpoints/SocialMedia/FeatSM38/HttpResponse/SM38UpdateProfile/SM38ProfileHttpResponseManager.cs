using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM38;
using PenomyAPI.App.SM38.GroupProfile;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM38Profile.HttpResponse;

public static class SM38ProfileHttpResponseManager
{
    private static ConcurrentDictionary<
        SM38ResponseStatusCode,
        Func<SM38ProfileResponse, SM38ProfileHttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: SM38ResponseStatusCode.SUCCESS,
            value: (response) =>
                new()
                {
                    AppCode = $"SM38.{SM38ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                }
        );

        _dictionary.TryAdd(
            key: SM38ResponseStatusCode.FAILED,
            value: (response) =>
                new()
                {
                    AppCode = $"SM38.{SM38ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );

        _dictionary.TryAdd(
            key: SM38ResponseStatusCode.FORBIDDEN,
            value: (response) =>
                new()
                {
                    AppCode = $"SM38.{SM38ResponseStatusCode.FORBIDDEN}",
                    HttpCode = StatusCodes.Status403Forbidden,
                }
        );

        _dictionary.TryAdd(
            key: SM38ResponseStatusCode.UN_AUTHORIZED,
            value: (response) =>
                new()
                {
                    AppCode = $"SM38.{SM38ResponseStatusCode.UN_AUTHORIZED}",
                    HttpCode = StatusCodes.Status401Unauthorized,
                }
        );
    }

    internal static Func<SM38ProfileResponse, SM38ProfileHttpResponse> Resolve(
        SM38ResponseStatusCode statusCode
    )
    {
        if (Equals(objA: _dictionary, objB: default))
        {
            Init();
        }

        if (_dictionary.TryGetValue(statusCode, out var response))
        {
            return response;
        }

        return _dictionary[statusCode];
    }
}
