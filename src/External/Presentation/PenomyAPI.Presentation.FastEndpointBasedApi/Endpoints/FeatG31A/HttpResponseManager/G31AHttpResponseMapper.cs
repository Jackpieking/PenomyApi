using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG31A;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG31A.HttpResponseManager;

internal static class G31AHttpResponseMapper
{
    private static ConcurrentDictionary<
        G31AResponseStatusCode,
        Func<G31ARequest, G31AResponse, G31AHttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: G31AResponseStatusCode.SUCCESS,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G31A.{G31AResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                    Body = response.Body
                }
        );

        _dictionary.TryAdd(
            key: G31AResponseStatusCode.DATABASE_ERROR,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G31A.{G31AResponseStatusCode.DATABASE_ERROR}",
                    HttpCode = StatusCodes.Status500InternalServerError,
                }
        );

        _dictionary.TryAdd(
            key: G31AResponseStatusCode.FORBIDDEN,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G31A.{G31AResponseStatusCode.FORBIDDEN}",
                    HttpCode = StatusCodes.Status403Forbidden,
                }
        );

        _dictionary.TryAdd(
            key: G31AResponseStatusCode.UN_AUTHORIZED,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G31A.{G31AResponseStatusCode.UN_AUTHORIZED}",
                    HttpCode = StatusCodes.Status429TooManyRequests,
                }
        );
    }

    internal static Func<G31ARequest, G31AResponse, G31AHttpResponse> Resolve(
        G31AResponseStatusCode statusCode
    )
    {
        if (Equals(_dictionary, default))
        {
            Init();
        }

        return _dictionary[statusCode];
    }
}
