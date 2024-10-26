using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG1.OtherHandlers.VerifyRegistrationToken;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.VerifyRegistrationToken.HttpResponseManager;

internal static class G1VerifyRegistrationTokenHttpResponseMapper
{
    private static ConcurrentDictionary<
        G1VerifyRegistrationTokenResponseStatusCode,
        Func<
            G1VerifyRegistrationTokenRequest,
            G1VerifyRegistrationTokenResponse,
            G1VerifyRegistrationTokenHttpResponse
        >
    > _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: G1VerifyRegistrationTokenResponseStatusCode.SUCCESS,
            value: (_, response) =>
                new()
                {
                    AppCode =
                        $"G1VerifyRegistrationToken.{G1VerifyRegistrationTokenResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                }
        );

        _dictionary.TryAdd(
            key: G1VerifyRegistrationTokenResponseStatusCode.INVALID_TOKEN,
            value: (_, response) =>
                new()
                {
                    AppCode =
                        $"G1VerifyRegistrationToken.{G1VerifyRegistrationTokenResponseStatusCode.INVALID_TOKEN}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );

        _dictionary.TryAdd(
            key: G1VerifyRegistrationTokenResponseStatusCode.USER_EXIST,
            value: (_, response) =>
                new()
                {
                    AppCode =
                        $"G1VerifyRegistrationToken.{G1VerifyRegistrationTokenResponseStatusCode.USER_EXIST}",
                    HttpCode = StatusCodes.Status409Conflict,
                }
        );
    }

    internal static Func<
        G1VerifyRegistrationTokenRequest,
        G1VerifyRegistrationTokenResponse,
        G1VerifyRegistrationTokenHttpResponse
    > Resolve(G1VerifyRegistrationTokenResponseStatusCode statusCode)
    {
        if (Equals(_dictionary, default))
        {
            Init();
        }

        return _dictionary[statusCode];
    }
}
