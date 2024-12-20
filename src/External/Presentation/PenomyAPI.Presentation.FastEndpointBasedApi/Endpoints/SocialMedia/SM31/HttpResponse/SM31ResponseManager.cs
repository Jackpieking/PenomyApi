using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM31;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.SM31.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM31.HttpResponse;

public class SM31ResponseManager
{
    private static ConcurrentDictionary<
        SM31ResponseStatusCode,
        Func<SM31Request, SM31Response, SM31HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary =
            new ConcurrentDictionary<SM31ResponseStatusCode, Func<SM31Request, SM31Response, SM31HttpResponse>>();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            SM31ResponseStatusCode.SUCCESS,
            (_, response) =>
                new SM31HttpResponse
                {
                    AppCode = $"SM31.{SM31ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK
                }
        );
        _dictionary.TryAdd(
            SM31ResponseStatusCode.USER_NOT_FOUND,
            (_, response) =>
                new SM31HttpResponse
                {
                    AppCode = $"SM31.{SM31ResponseStatusCode.USER_NOT_FOUND}",
                    HttpCode = StatusCodes.Status400BadRequest
                }
        );
        _dictionary.TryAdd(
            SM31ResponseStatusCode.IS_NOT_FRIEND,
            (_, response) =>
                new SM31HttpResponse
                {
                    AppCode = $"SM31.{SM31ResponseStatusCode.IS_NOT_FRIEND}",
                    HttpCode = StatusCodes.Status400BadRequest
                }
        );
        _dictionary.TryAdd(
            SM31ResponseStatusCode.FAILED,
            (_, response) =>
                new SM31HttpResponse
                {
                    AppCode = $"SM31.{SM31ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status400BadRequest
                }
        );
        _dictionary.TryAdd(
            SM31ResponseStatusCode.UNAUTHORIZED,
            (_, response) =>
                new SM31HttpResponse
                {
                    AppCode = $"SM31.{SM31ResponseStatusCode.UNAUTHORIZED}",
                    HttpCode = StatusCodes.Status401Unauthorized
                }
        );
    }

    internal static Func<SM31Request, SM31Response, SM31HttpResponse> Resolve(
        SM31ResponseStatusCode statusCode
    )
    {
        if (Equals(_dictionary, default)) Init();

        return _dictionary[statusCode];
    }
}
