using Microsoft.AspNetCore.Http;
using PenomyAPI.App.G45;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G45.DTOs;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G45.HttpResponse;

public class G45ResponseManager
{
    private static ConcurrentDictionary<
        G45ResponseStatusCode,
        Func<G45Request, G45Response, G45HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: G45ResponseStatusCode.SUCCESS,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G45.{G45ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                    Body = response.Result.Select(G45FollowedArtworkResponseDto.MapFrom)
                }
        );

        _dictionary.TryAdd(
            key: G45ResponseStatusCode.FAILED,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G45.{G45ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status500InternalServerError,
                }
        );
        _dictionary.TryAdd(
            key: G45ResponseStatusCode.INVALID_REQUEST,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G45.{G45ResponseStatusCode.INVALID_REQUEST}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );
    }

    internal static Func<G45Request, G45Response, G45HttpResponse> Resolve(
        G45ResponseStatusCode statusCode
    )
    {
        if (Equals(objA: _dictionary, objB: default))
        {
            Init();
        }

        return _dictionary[statusCode];
    }
}
