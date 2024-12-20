using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM14;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM14.HttpResponse;

public class SM14HttpResponseManager
{
    private static ConcurrentDictionary<
        SM14ResponseStatusCode,
        Func<SM14Response, SM14HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new ConcurrentDictionary<SM14ResponseStatusCode, Func<SM14Response, SM14HttpResponse>>();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            SM14ResponseStatusCode.SUCCESS,
            response => new SM14HttpResponse
            {
                AppCode = SM14HttpResponse.GetAppCode(SM14ResponseStatusCode.SUCCESS),
                HttpCode = StatusCodes.Status200OK,
                Body = null
            });
        ;

        _dictionary.TryAdd(
            SM14ResponseStatusCode.POST_NOT_FOUND,
            response => new SM14HttpResponse
            {
                AppCode = SM14HttpResponse.GetAppCode(SM14ResponseStatusCode.POST_NOT_FOUND),
                HttpCode = StatusCodes.Status400BadRequest
            });

        _dictionary.TryAdd(
            SM14ResponseStatusCode.DATABASE_ERROR,
            response => new SM14HttpResponse
            {
                AppCode = SM14HttpResponse.GetAppCode(SM14ResponseStatusCode.DATABASE_ERROR),
                HttpCode = StatusCodes.Status500InternalServerError
            });
        _dictionary.TryAdd(
            SM14ResponseStatusCode.DATABASE_ERROR,
            response => new SM14HttpResponse
            {
                AppCode = SM14HttpResponse.GetAppCode(SM14ResponseStatusCode.FILE_SERVICE_ERROR),
                HttpCode = StatusCodes.Status500InternalServerError
            });
    }

    internal static Func<SM14Response, SM14HttpResponse> Resolve(SM14ResponseStatusCode statusCode)
    {
        if (Equals(_dictionary, default)) Init();

        var keyExisted = _dictionary.TryGetValue(statusCode, out var value);

        if (keyExisted) return value;

        return _dictionary[SM14ResponseStatusCode.POST_NOT_FOUND];
    }
}
