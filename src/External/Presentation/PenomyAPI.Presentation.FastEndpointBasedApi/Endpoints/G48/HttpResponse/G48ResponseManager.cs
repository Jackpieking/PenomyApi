using Microsoft.AspNetCore.Http;
using PenomyAPI.App.G48;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G48.DTOs;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G48.HttpResponse;

public class G48ResponseManager
{
    private static ConcurrentDictionary<
        G48ResponseStatusCode,
        Func<G48Request, G48Response, G48HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: G48ResponseStatusCode.SUCCESS,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G48.{G48ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                    Body = response.Result.Select(G48FavoriteArtworkResponseDto.MapFrom)
                }
        );

        _dictionary.TryAdd(
            key: G48ResponseStatusCode.FAILED,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G48.{G48ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status500InternalServerError,
                }
        );
        _dictionary.TryAdd(
            key: G48ResponseStatusCode.INVALID_REQUEST,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G48.{G48ResponseStatusCode.INVALID_REQUEST}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );
    }

    internal static Func<G48Request, G48Response, G48HttpResponse> Resolve(
        G48ResponseStatusCode statusCode
    )
    {
        if (Equals(objA: _dictionary, objB: default))
        {
            Init();
        }

        return _dictionary[statusCode];
    }
}
