using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG34;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG34.InitResetPassword.HttpResponseManager;

internal static class G34HttpResponseMapper
{
    private static ConcurrentDictionary<
        G34ResponseStatusCode,
        Func<G34Request, G34Response, G34HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: G34ResponseStatusCode.SUCCESS,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G34.{G34ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                }
        );

        _dictionary.TryAdd(
            key: G34ResponseStatusCode.SENDING_MAIL_FAILED,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G34.{G34ResponseStatusCode.SENDING_MAIL_FAILED}",
                    HttpCode = StatusCodes.Status500InternalServerError,
                }
        );

        _dictionary.TryAdd(
            key: G34ResponseStatusCode.USER_NOT_EXIST,
            value: (request, response) =>
                new()
                {
                    AppCode = $"G34.{G34ResponseStatusCode.USER_NOT_EXIST}",
                    HttpCode = StatusCodes.Status404NotFound
                }
        );
    }

    internal static Func<G34Request, G34Response, G34HttpResponse> Resolve(
        G34ResponseStatusCode statusCode
    )
    {
        if (Equals(_dictionary, default))
        {
            Init();
        }

        return _dictionary[statusCode];
    }
}
