using System;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.AspNetCore.Http;
using PenomyAPI.APP.FeatG6;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG6.DTOs;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG6.HttpResponse;

public class G6HttpResponseManager
{
    private static ConcurrentDictionary<
        G6ResponseStatusCode,
        Func<G6Request, G6Response, G6HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: G6ResponseStatusCode.SUCCESS,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G6.{G6ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                    Body = response.Result.Select(G6RecommendedArtworkResponseDto.MapFrom)
                }
        );

        _dictionary.TryAdd(
            key: G6ResponseStatusCode.ARTWORK_ID_NOT_FOUND,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G6.{G6ResponseStatusCode.ARTWORK_ID_NOT_FOUND}",
                    HttpCode = StatusCodes.Status404NotFound,
                }
        );

        _dictionary.TryAdd(
            key: G6ResponseStatusCode.DATABASE_ERROR,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G6.{G6ResponseStatusCode.DATABASE_ERROR}",
                    HttpCode = StatusCodes.Status500InternalServerError,
                }
        );
    }

    internal static Func<G6Request, G6Response, G6HttpResponse> Resolve(
        G6ResponseStatusCode statusCode
    )
    {
        if (Equals(objA: _dictionary, objB: default))
        {
            Init();
        }

        return _dictionary[statusCode];
    }
}
