using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG34.OtherHandlers.VerifyResetPasswordToken;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG34.VerifyResetPasswordToken.HttpResponseManager;

public static class G34VerifyResetPasswordTokenHttpResponseMapper
{
    private static ConcurrentDictionary<
        G34VerifyResetPasswordTokenResponseStatusCode,
        Func<
            G34VerifyResetPasswordTokenRequest,
            G34VerifyResetPasswordTokenResponse,
            G34VerifyResetPasswordTokenHttpResponse
        >
    > _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: G34VerifyResetPasswordTokenResponseStatusCode.SUCCESS,
            value: (_, response) =>
                new()
                {
                    AppCode =
                        $"G34VerifyResetPasswordToken.{G34VerifyResetPasswordTokenResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                    Body = response.ResetPasswordToken
                }
        );

        _dictionary.TryAdd(
            key: G34VerifyResetPasswordTokenResponseStatusCode.INVALID_TOKEN,
            value: (_, response) =>
                new()
                {
                    AppCode =
                        $"G34VerifyResetPasswordToken.{G34VerifyResetPasswordTokenResponseStatusCode.INVALID_TOKEN}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );

        _dictionary.TryAdd(
            key: G34VerifyResetPasswordTokenResponseStatusCode.DATABASE_ERROR,
            value: (_, response) =>
                new()
                {
                    AppCode =
                        $"G34VerifyResetPasswordToken.{G34VerifyResetPasswordTokenResponseStatusCode.DATABASE_ERROR}",
                    HttpCode = StatusCodes.Status500InternalServerError,
                }
        );
    }

    internal static Func<
        G34VerifyResetPasswordTokenRequest,
        G34VerifyResetPasswordTokenResponse,
        G34VerifyResetPasswordTokenHttpResponse
    > Resolve(G34VerifyResetPasswordTokenResponseStatusCode statusCode)
    {
        if (Equals(_dictionary, default))
        {
            Init();
        }

        return _dictionary[statusCode];
    }
}
