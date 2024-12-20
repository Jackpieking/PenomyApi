using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenowmyAPI.APP.Chat2;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Chat.Chat2.HttpResponse;

public class Chat2HttpResponseManager
{
    private static ConcurrentDictionary<
        Chat2ResponseStatusCode,
        Func<Chat2Request, Chat2Response, Chat2HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary =
            new ConcurrentDictionary<Chat2ResponseStatusCode, Func<Chat2Request, Chat2Response, Chat2HttpResponse>>();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            Chat2ResponseStatusCode.SUCCESS,
            (_, response) =>
                new Chat2HttpResponse
                {
                    AppCode = $"Chat2.{Chat2ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK
                }
        );

        _dictionary.TryAdd(
            Chat2ResponseStatusCode.FAILED,
            (_, response) =>
                new Chat2HttpResponse
                {
                    AppCode = $"Chat2.{Chat2ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status400BadRequest
                }
        );
        _dictionary.TryAdd(
            Chat2ResponseStatusCode.UNAUTHORIZED,
            (_, response) =>
                new Chat2HttpResponse
                {
                    AppCode = $"Chat2.{Chat2ResponseStatusCode.UNAUTHORIZED}",
                    HttpCode = StatusCodes.Status401Unauthorized
                }
        );
    }

    internal static Func<Chat2Request, Chat2Response, Chat2HttpResponse> Resolve(
        Chat2ResponseStatusCode statusCode
    )
    {
        if (Equals(_dictionary, default)) Init();

        return _dictionary[statusCode];
    }
}
