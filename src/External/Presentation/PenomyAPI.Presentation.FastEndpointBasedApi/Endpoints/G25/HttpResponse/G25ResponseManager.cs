using Microsoft.AspNetCore.Http;
using PenomyAPI.App.G25;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25.DTOs;
using System;
using System.Collections.Concurrent;
using System.Linq;

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
                    Body = response.Result.Select(G25ArtworkViewHistoryItemResponseDto.MapFrom)
                }
        );

        _dictionary.TryAdd(
            key: G25ResponseStatusCode.INVALID_REQUEST,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G25.{G25ResponseStatusCode.INVALID_REQUEST}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );

        _dictionary.TryAdd(
            key: G25ResponseStatusCode.FAILED,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G25.{G25ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );

        _dictionary.TryAdd(
            key: G25ResponseStatusCode.UN_AUTHORIZED,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G25.{G25ResponseStatusCode.UN_AUTHORIZED}",
                    HttpCode = StatusCodes.Status401Unauthorized,
                }
        );

        _dictionary.TryAdd(
            key: G25ResponseStatusCode.FORBIDDEN,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G25.{G25ResponseStatusCode.FORBIDDEN}",
                    HttpCode = StatusCodes.Status403Forbidden,
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
