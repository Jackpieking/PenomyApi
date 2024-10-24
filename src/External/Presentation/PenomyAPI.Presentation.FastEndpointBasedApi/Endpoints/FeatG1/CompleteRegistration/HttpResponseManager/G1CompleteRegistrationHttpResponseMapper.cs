using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG1.OtherHandlers.CompleteRegistration;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.CompleteRegistration.HttpResponseManager;

internal static class G1CompleteRegistrationHttpResponseMapper
{
    private static ConcurrentDictionary<
        G1CompleteRegistrationResponseStatusCode,
        Func<
            G1CompleteRegistrationRequest,
            G1CompleteRegistrationResponse,
            G1CompleteRegistrationHttpResponse
        >
    > _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: G1CompleteRegistrationResponseStatusCode.SUCCESS,
            value: (_, response) =>
                new()
                {
                    AppCode =
                        $"G1CompleteRegistration.{G1CompleteRegistrationResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                }
        );

        _dictionary.TryAdd(
            key: G1CompleteRegistrationResponseStatusCode.DATABASE_ERROR,
            value: (_, response) =>
                new()
                {
                    AppCode =
                        $"G1CompleteRegistration.{G1CompleteRegistrationResponseStatusCode.DATABASE_ERROR}",
                    HttpCode = StatusCodes.Status500InternalServerError,
                }
        );

        _dictionary.TryAdd(
            key: G1CompleteRegistrationResponseStatusCode.PASSWORD_INVALID,
            value: (_, response) =>
                new()
                {
                    AppCode =
                        $"G1CompleteRegistration.{G1CompleteRegistrationResponseStatusCode.PASSWORD_INVALID}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );

        _dictionary.TryAdd(
            key: G1CompleteRegistrationResponseStatusCode.INVALID_TOKEN,
            value: (_, response) =>
                new()
                {
                    AppCode =
                        $"G1CompleteRegistration.{G1CompleteRegistrationResponseStatusCode.INVALID_TOKEN}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );

        _dictionary.TryAdd(
            key: G1CompleteRegistrationResponseStatusCode.USER_EXIST,
            value: (_, response) =>
                new()
                {
                    AppCode =
                        $"G1CompleteRegistration.{G1CompleteRegistrationResponseStatusCode.USER_EXIST}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );
    }

    internal static Func<
        G1CompleteRegistrationRequest,
        G1CompleteRegistrationResponse,
        G1CompleteRegistrationHttpResponse
    > Resolve(G1CompleteRegistrationResponseStatusCode statusCode)
    {
        if (Equals(_dictionary, default))
        {
            Init();
        }

        return _dictionary[statusCode];
    }
}
