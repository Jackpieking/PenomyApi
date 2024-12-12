using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM30;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.FeatSM30.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM30.HttpResponse;

public class SM30ResponseManager
{
    private static ConcurrentDictionary<
        SM30ResponseStatusCode,
        Func<SM30Request, SM30Response, SM30HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary =
            new ConcurrentDictionary<SM30ResponseStatusCode, Func<SM30Request, SM30Response, SM30HttpResponse>>();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            SM30ResponseStatusCode.ALREADY_SENT_REQUEST,
            (_, response) =>
                new SM30HttpResponse
                {
                    AppCode = $"SM30.{SM30ResponseStatusCode.ALREADY_SENT_REQUEST}",
                    HttpCode = StatusCodes.Status400BadRequest
                }
        );
        _dictionary.TryAdd(
            SM30ResponseStatusCode.USER_NOT_FOUND,
            (_, response) =>
                new SM30HttpResponse
                {
                    AppCode = $"SM30.{SM30ResponseStatusCode.USER_NOT_FOUND}",
                    HttpCode = StatusCodes.Status400BadRequest
                }
        );
        _dictionary.TryAdd(
            SM30ResponseStatusCode.SUCCESS,
            (_, response) =>
                new SM30HttpResponse
                {
                    AppCode = $"SM30.{SM30ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK
                }
        );

        _dictionary.TryAdd(
            SM30ResponseStatusCode.FAILED,
            (_, response) =>
                new SM30HttpResponse
                {
                    AppCode = $"SM30.{SM30ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status400BadRequest
                }
        );
        _dictionary.TryAdd(
            SM30ResponseStatusCode.UNAUTHORIZED,
            (_, response) =>
                new SM30HttpResponse
                {
                    AppCode = $"SM30.{SM30ResponseStatusCode.UNAUTHORIZED}",
                    HttpCode = StatusCodes.Status401Unauthorized
                }
        );
        _dictionary.TryAdd(
            SM30ResponseStatusCode.ALREADY_FRIEND,
            (_, response) =>
                new SM30HttpResponse
                {
                    AppCode = $"SM30.{SM30ResponseStatusCode.ALREADY_FRIEND}",
                    HttpCode = StatusCodes.Status400BadRequest
                }
        );
    }

    internal static Func<SM30Request, SM30Response, SM30HttpResponse> Resolve(
        SM30ResponseStatusCode statusCode
    )
    {
        if (Equals(_dictionary, default)) Init();

        return _dictionary[statusCode];
    }
}
