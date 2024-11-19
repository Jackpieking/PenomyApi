using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG28.PageCount;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G28.PageCount.HttpResponse;

public static class G28PageCountHttpResponseManager
{
    private static ConcurrentDictionary<
        G28PageCountResponseStatusCode,
        Func<G28PageCountRequest, G28PageCountResponse, G28PageCountHttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: G28PageCountResponseStatusCode.SUCCESS,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G28.{G28PageCountResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                }
        );

        _dictionary.TryAdd(
            key: G28PageCountResponseStatusCode.DATABASE_ERROR,
            value: (_, response) =>
                new()
                {
                    AppCode = $"G28.{G28PageCountResponseStatusCode.DATABASE_ERROR}",
                    HttpCode = StatusCodes.Status400BadRequest,
                }
        );
    }

    internal static Func<
        G28PageCountRequest,
        G28PageCountResponse,
        G28PageCountHttpResponse
    > Resolve(G28PageCountResponseStatusCode statusCode)
    {
        if (Equals(objA: _dictionary, objB: default))
        {
            Init();
        }

        return _dictionary[statusCode];
    }
}
