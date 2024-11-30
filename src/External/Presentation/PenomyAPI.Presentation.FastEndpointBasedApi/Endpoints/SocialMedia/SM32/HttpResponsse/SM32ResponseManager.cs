using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM32;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.SM32.HttpResponsse;

public class SM32ResponseManager
{
    private static ConcurrentDictionary<
        SM32ResponseStatusCode,
        Func<SM32Request, SM32Response, SM32HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary =
            new ConcurrentDictionary<SM32ResponseStatusCode, Func<SM32Request, SM32Response, SM32HttpResponse>>();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            SM32ResponseStatusCode.SUCCESS,
            (_, response) =>
                new SM32HttpResponse
                {
                    AppCode = $"SM32.{SM32ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK
                }
        );

        _dictionary.TryAdd(
            SM32ResponseStatusCode.FAILED,
            (_, response) =>
                new SM32HttpResponse
                {
                    AppCode = $"SM32.{SM32ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status500InternalServerError
                }
        );
        _dictionary.TryAdd(
            SM32ResponseStatusCode.UNAUTHORIZED,
            (_, response) =>
                new SM32HttpResponse
                {
                    AppCode = $"SM32.{SM32ResponseStatusCode.UNAUTHORIZED}",
                    HttpCode = StatusCodes.Status401Unauthorized
                }
        );
    }

    internal static Func<SM32Request, SM32Response, SM32HttpResponse> Resolve(
        SM32ResponseStatusCode statusCode
    )
    {
        if (Equals(_dictionary, default)) Init();

        return _dictionary[statusCode];
    }
}
