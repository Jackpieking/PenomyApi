using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM15;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.SM15.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM15.HttpResponse;

public class SM15ResponseManager
{
    private static ConcurrentDictionary<
        SM15ResponseStatusCode,
        Func<SM15Request, SM15Response, Sm15HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary =
            new ConcurrentDictionary<
                SM15ResponseStatusCode,
                Func<SM15Request, SM15Response, Sm15HttpResponse>
            >();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            SM15ResponseStatusCode.SUCCESS,
            (_, response) =>
                new Sm15HttpResponse
                {
                    AppCode = $"SM15.{SM15ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                }
        );

        _dictionary.TryAdd(
            SM15ResponseStatusCode.FAILED,
            (_, response) =>
                new Sm15HttpResponse
                {
                    AppCode = $"SM15.{SM15ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );
        _dictionary.TryAdd(
            SM15ResponseStatusCode.UNAUTHORIZED,
            (_, response) =>
                new Sm15HttpResponse
                {
                    AppCode = $"SM15.{SM15ResponseStatusCode.UNAUTHORIZED}",
                    HttpCode = StatusCodes.Status401Unauthorized,
                }
        );
    }

    internal static Func<SM15Request, SM15Response, Sm15HttpResponse> Resolve(
        SM15ResponseStatusCode statusCode
    )
    {
        if (Equals(_dictionary, default))
            Init();

        return _dictionary[statusCode];
    }
}
