using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM50;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.SM50.HttpResponse;

public class SM50ResponseManager
{
    private static ConcurrentDictionary<
        SM50ResponseStatusCode,
        Func<SM50Request, SM50Response, SM50HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary =
            new ConcurrentDictionary<
                SM50ResponseStatusCode,
                Func<SM50Request, SM50Response, SM50HttpResponse>
            >();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            SM50ResponseStatusCode.SUCCESS,
            (_, response) =>
                new SM50HttpResponse
                {
                    AppCode = $"SM50.{SM50ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                }
        );

        _dictionary.TryAdd(
            SM50ResponseStatusCode.FAILED,
            (_, response) =>
                new SM50HttpResponse
                {
                    AppCode = $"SM50.{SM50ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status500InternalServerError,
                }
        );
        _dictionary.TryAdd(
            SM50ResponseStatusCode.UNAUTHORIZED,
            (_, response) =>
                new SM50HttpResponse
                {
                    AppCode = $"SM50.{SM50ResponseStatusCode.UNAUTHORIZED}",
                    HttpCode = StatusCodes.Status401Unauthorized,
                }
        );

        _dictionary.TryAdd(
            SM50ResponseStatusCode.ALREADY_FRIEND,
            (_, response) =>
                new SM50HttpResponse
                {
                    AppCode = $"SM50.{SM50ResponseStatusCode.ALREADY_FRIEND}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );
    }

    internal static Func<SM50Request, SM50Response, SM50HttpResponse> Resolve(
        SM50ResponseStatusCode statusCode
    )
    {
        if (Equals(_dictionary, default))
            Init();

        return _dictionary[statusCode];
    }
}
