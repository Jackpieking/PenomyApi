using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG1;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.InitRegistration.HttpResponseManager;

internal static class G1HttpResponseMapper
{
    private static ConcurrentDictionary<
        G1ResponseStatusCode,
        Func<G1Request, G1Response, G1HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: G1ResponseStatusCode.SUCCESS,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G1.{G1ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                }
        );

        _dictionary.TryAdd(
            key: G1ResponseStatusCode.SENDING_MAIL_FAILED,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G1.{G1ResponseStatusCode.SENDING_MAIL_FAILED}",
                    HttpCode = StatusCodes.Status500InternalServerError,
                }
        );

        _dictionary.TryAdd(
            key: G1ResponseStatusCode.USER_EXIST,
            value: (request, response) =>
                new()
                {
                    AppCode = $"G1.{G1ResponseStatusCode.USER_EXIST}",
                    HttpCode = StatusCodes.Status409Conflict
                }
        );
    }

    internal static Func<G1Request, G1Response, G1HttpResponse> Resolve(
        G1ResponseStatusCode statusCode
    )
    {
        if (Equals(_dictionary, default))
        {
            Init();
        }

        return _dictionary[statusCode];
    }
}
