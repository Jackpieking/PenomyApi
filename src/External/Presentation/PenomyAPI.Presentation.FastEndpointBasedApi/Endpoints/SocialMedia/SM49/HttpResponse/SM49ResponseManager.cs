using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM49;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.SM49.HttpResponse;

public class SM49ResponseManager
{
    private static ConcurrentDictionary<
        SM49ResponseStatusCode,
        Func<SM49Request, SM49Response, SM49HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary =
            new ConcurrentDictionary<
                SM49ResponseStatusCode,
                Func<SM49Request, SM49Response, SM49HttpResponse>
            >();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            SM49ResponseStatusCode.SUCCESS,
            (_, response) =>
                new SM49HttpResponse
                {
                    AppCode = $"SM49.{SM49ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                }
        );

        _dictionary.TryAdd(
            SM49ResponseStatusCode.FAILED,
            (_, response) =>
                new SM49HttpResponse
                {
                    AppCode = $"SM49.{SM49ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status500InternalServerError,
                }
        );
        _dictionary.TryAdd(
            SM49ResponseStatusCode.UNAUTHORIZED,
            (_, response) =>
                new SM49HttpResponse
                {
                    AppCode = $"SM49.{SM49ResponseStatusCode.UNAUTHORIZED}",
                    HttpCode = StatusCodes.Status401Unauthorized,
                }
        );

        _dictionary.TryAdd(
            SM49ResponseStatusCode.ALREADY_FRIEND,
            (_, response) =>
                new SM49HttpResponse
                {
                    AppCode = $"SM49.{SM49ResponseStatusCode.ALREADY_FRIEND}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );
    }

    internal static Func<SM49Request, SM49Response, SM49HttpResponse> Resolve(
        SM49ResponseStatusCode statusCode
    )
    {
        if (Equals(_dictionary, default))
            Init();

        return _dictionary[statusCode];
    }
}
