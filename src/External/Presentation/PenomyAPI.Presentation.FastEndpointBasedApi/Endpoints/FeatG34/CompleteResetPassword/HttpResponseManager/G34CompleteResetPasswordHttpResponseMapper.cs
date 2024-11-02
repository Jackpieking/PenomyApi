using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG34.OtherHandlers.CompleteResetPassword;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG34.CompleteResetPassword.HttpResponseManager;

public static class G34CompleteResetPasswordHttpResponseMapper
{
    private static ConcurrentDictionary<
        G34CompleteResetPasswordResponseStatusCode,
        Func<
            G34CompleteResetPasswordRequest,
            G34CompleteResetPasswordResponse,
            G34CompleteResetPasswordHttpResponse
        >
    > _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: G34CompleteResetPasswordResponseStatusCode.SUCCESS,
            value: (_, response) =>
                new()
                {
                    AppCode =
                        $"G34CompleteResetPassword.{G34CompleteResetPasswordResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK
                }
        );

        _dictionary.TryAdd(
            key: G34CompleteResetPasswordResponseStatusCode.INVALID_TOKEN,
            value: (_, response) =>
                new()
                {
                    AppCode =
                        $"G34CompleteResetPassword.{G34CompleteResetPasswordResponseStatusCode.INVALID_TOKEN}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );

        _dictionary.TryAdd(
            key: G34CompleteResetPasswordResponseStatusCode.INVALID_PASSWORD,
            value: (_, response) =>
                new()
                {
                    AppCode =
                        $"G34CompleteResetPassword.{G34CompleteResetPasswordResponseStatusCode.INVALID_PASSWORD}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );

        _dictionary.TryAdd(
            key: G34CompleteResetPasswordResponseStatusCode.DATABASE_ERROR,
            value: (_, response) =>
                new()
                {
                    AppCode =
                        $"G34CompleteResetPassword.{G34CompleteResetPasswordResponseStatusCode.DATABASE_ERROR}",
                    HttpCode = StatusCodes.Status500InternalServerError,
                }
        );
    }

    internal static Func<
        G34CompleteResetPasswordRequest,
        G34CompleteResetPasswordResponse,
        G34CompleteResetPasswordHttpResponse
    > Resolve(G34CompleteResetPasswordResponseStatusCode statusCode)
    {
        if (Equals(_dictionary, default))
        {
            Init();
        }

        return _dictionary[statusCode];
    }
}
