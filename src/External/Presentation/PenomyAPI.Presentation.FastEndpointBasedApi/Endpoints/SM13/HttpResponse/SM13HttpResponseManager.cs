using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM13;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM13.HttpResponse;

public class SM13HttpResponseManager
{
    private static ConcurrentDictionary<
        SM13ResponseStatusCode,
        Func<SM13Response, SM13HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new ConcurrentDictionary<SM13ResponseStatusCode, Func<SM13Response, SM13HttpResponse>>();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            SM13ResponseStatusCode.SUCCESS,
            response => new SM13HttpResponse
            {
                AppCode = SM13HttpResponse.GetAppCode(SM13ResponseStatusCode.SUCCESS),
                HttpCode = StatusCodes.Status200OK,
                Body = null
            });

        _dictionary.TryAdd(
            SM13ResponseStatusCode.DATABASE_ERROR,
            response => new SM13HttpResponse
            {
                AppCode = SM13HttpResponse.GetAppCode(SM13ResponseStatusCode.DATABASE_ERROR),
                HttpCode = StatusCodes.Status500InternalServerError
            });

        _dictionary.TryAdd(
            SM13ResponseStatusCode.FILE_SERVICE_ERROR,
            response => new SM13HttpResponse
            {
                AppCode = SM13HttpResponse.GetAppCode(SM13ResponseStatusCode.FILE_SERVICE_ERROR),
                HttpCode = StatusCodes.Status500InternalServerError
            });


        _dictionary.TryAdd(
            SM13ResponseStatusCode.INVALID_FILE_EXTENSION,
            response => new SM13HttpResponse
            {
                AppCode = SM13HttpResponse.GetAppCode(SM13ResponseStatusCode.INVALID_FILE_EXTENSION),
                HttpCode = StatusCodes.Status400BadRequest
            });

        _dictionary.TryAdd(
            SM13ResponseStatusCode.INVALID_FILE_FORMAT,
            response => new SM13HttpResponse
            {
                AppCode = SM13HttpResponse.GetAppCode(SM13ResponseStatusCode.INVALID_FILE_FORMAT),
                HttpCode = StatusCodes.Status400BadRequest
            });

        _dictionary.TryAdd(
            SM13ResponseStatusCode.FILE_SIZE_IS_EXCEED_THE_LIMIT,
            response => new SM13HttpResponse
            {
                AppCode = SM13HttpResponse.GetAppCode(SM13ResponseStatusCode.FILE_SIZE_IS_EXCEED_THE_LIMIT),
                HttpCode = StatusCodes.Status400BadRequest
            });
        _dictionary.TryAdd(
            SM13ResponseStatusCode.USER_POST_NOT_FOUND,
            response => new SM13HttpResponse
            {
                AppCode = SM13HttpResponse.GetAppCode(SM13ResponseStatusCode.USER_POST_NOT_FOUND),
                HttpCode = StatusCodes.Status400BadRequest
            });
    }

    internal static Func<SM13Response, SM13HttpResponse> Resolve(SM13ResponseStatusCode statusCode)
    {
        if (Equals(_dictionary, default)) Init();

        var keyExisted = _dictionary.TryGetValue(statusCode, out var value);

        if (keyExisted) return value;

        return _dictionary[SM13ResponseStatusCode.FILE_SERVICE_ERROR];
    }
}
