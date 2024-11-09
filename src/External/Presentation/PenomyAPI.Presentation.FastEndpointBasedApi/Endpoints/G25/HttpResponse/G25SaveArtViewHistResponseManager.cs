using Microsoft.AspNetCore.Http;
using PenomyAPI.App.G25;
using PenomyAPI.App.G25.OtherHandlers.SaveArtViewHist;
using System;
using System.Collections.Concurrent;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25.HttpResponse;

public class G25SaveArtViewHistResponseManager
{
    private static ConcurrentDictionary<
        G25ResponseStatusCode,
        Func<G25SaveArtViewHistRequest, G25SaveArtViewHistResponse, G25SaveArtViewHistHttpResponse>
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
            key: G25ResponseStatusCode.SUCCESS,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G25.{G25ResponseStatusCode.INVALID_REQUEST}",
                    HttpCode = StatusCodes.Status200OK,
                }
        );

        _dictionary.TryAdd(
            key: G25ResponseStatusCode.SUCCESS,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G25.{G25ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status200OK,
                }
        );

        _dictionary.TryAdd(
            key: G25ResponseStatusCode.SUCCESS,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G25.{G25ResponseStatusCode.UN_AUTHORIZED}",
                    HttpCode = StatusCodes.Status200OK,
                }
        );

        _dictionary.TryAdd(
            key: G25ResponseStatusCode.FAILED,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G25.{G25ResponseStatusCode.FORBIDDEN}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );
    }

    internal static Func<
        G25SaveArtViewHistRequest,
        G25SaveArtViewHistResponse,
        G25SaveArtViewHistHttpResponse
    > Resolve(G25ResponseStatusCode statusCode)
    {
        if (Equals(objA: _dictionary, objB: default))
        {
            Init();
        }

        return _dictionary[statusCode];
    }
}
