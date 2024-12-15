using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM11;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.SM11.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM11.HttpResponse;

public class SM11ResponseManager
{
    private static ConcurrentDictionary<
        SM11ResponseStatusCode,
        Func<SM11Request, SM11Response, Sm11HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary =
            new ConcurrentDictionary<
                SM11ResponseStatusCode,
                Func<SM11Request, SM11Response, Sm11HttpResponse>
            >();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            SM11ResponseStatusCode.SUCCESS,
            (_, response) =>
                new Sm11HttpResponse
                {
                    AppCode = $"SM11.{SM11ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                }
        );

        _dictionary.TryAdd(
            SM11ResponseStatusCode.FAILED,
            (_, response) =>
                new Sm11HttpResponse
                {
                    AppCode = $"SM11.{SM11ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );
        _dictionary.TryAdd(
            SM11ResponseStatusCode.UNAUTHORIZED,
            (_, response) =>
                new Sm11HttpResponse
                {
                    AppCode = $"SM11.{SM11ResponseStatusCode.UNAUTHORIZED}",
                    HttpCode = StatusCodes.Status401Unauthorized,
                }
        );
    }

    internal static Func<SM11Request, SM11Response, Sm11HttpResponse> Resolve(
        SM11ResponseStatusCode statusCode
    )
    {
        if (Equals(_dictionary, default))
            Init();

        return _dictionary[statusCode];
    }
}
