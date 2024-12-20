using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM30.SM30UnsendHandler;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.SM30Unsend.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.SM30UnsendUnsend.HttpResponse;

public class SM30UnsendHttpResponseManager
{
    private static ConcurrentDictionary<
        SM30UnsendResponseStatusCode,
        Func<SM30UnsendRequest, SM30UnsendResponse, SM30UnsendHttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary =
            new ConcurrentDictionary<SM30UnsendResponseStatusCode,
                Func<SM30UnsendRequest, SM30UnsendResponse, SM30UnsendHttpResponse>>();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            SM30UnsendResponseStatusCode.NOT_SEND,
            (_, response) =>
                new SM30UnsendHttpResponse
                {
                    AppCode = $"SM30Unsend.{SM30UnsendResponseStatusCode.NOT_SEND}",
                    HttpCode = StatusCodes.Status400BadRequest
                }
        );
        _dictionary.TryAdd(
            SM30UnsendResponseStatusCode.SUCCESS,
            (_, response) =>
                new SM30UnsendHttpResponse
                {
                    AppCode = $"SM30Unsend.{SM30UnsendResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK
                }
        );

        _dictionary.TryAdd(
            SM30UnsendResponseStatusCode.FAILED,
            (_, response) =>
                new SM30UnsendHttpResponse
                {
                    AppCode = $"SM30Unsend.{SM30UnsendResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status400BadRequest
                }
        );
        _dictionary.TryAdd(
            SM30UnsendResponseStatusCode.UNAUTHORIZED,
            (_, response) =>
                new SM30UnsendHttpResponse
                {
                    AppCode = $"SM30Unsend.{SM30UnsendResponseStatusCode.UNAUTHORIZED}",
                    HttpCode = StatusCodes.Status401Unauthorized
                }
        );
    }

    internal static Func<SM30UnsendRequest, SM30UnsendResponse, SM30UnsendHttpResponse> Resolve(
        SM30UnsendResponseStatusCode statusCode
    )
    {
        if (Equals(_dictionary, default)) Init();

        return _dictionary[statusCode];
    }
}
