using Microsoft.AspNetCore.Http;
using PenomyAPI.App.Chat10;
using System;
using System.Collections.Concurrent;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Chat.Chat10.HttpResponse;

public class Chat10HttpResponseManager
{
    private static ConcurrentDictionary<
        Chat10ResponseStatusCode,
        Func<Chat10Response, Chat10HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new ConcurrentDictionary<Chat10ResponseStatusCode, Func<Chat10Response, Chat10HttpResponse>>();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            Chat10ResponseStatusCode.SUCCESS,
            response =>
                new Chat10HttpResponse
                {
                    AppCode = $"Chat10.{Chat10ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK
                }
        );

        _dictionary.TryAdd(
            Chat10ResponseStatusCode.INVALID_REQUEST,
            response =>
                new Chat10HttpResponse
                {
                    AppCode = $"Chat10.{Chat10ResponseStatusCode.INVALID_REQUEST}",
                    HttpCode = StatusCodes.Status400BadRequest
                }
        );

        _dictionary.TryAdd(
            Chat10ResponseStatusCode.DATABASE_ERROR,
            response =>
                new Chat10HttpResponse
                {
                    AppCode = $"Chat10.{Chat10ResponseStatusCode.DATABASE_ERROR}",
                    HttpCode = StatusCodes.Status400BadRequest
                }
        );

        _dictionary.TryAdd(
            Chat10ResponseStatusCode.FAILED,
            response =>
                new Chat10HttpResponse
                {
                    AppCode = $"Chat10.{Chat10ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status400BadRequest
                }
        );

        _dictionary.TryAdd(
            Chat10ResponseStatusCode.FORBIDDEN,
            response =>
                new Chat10HttpResponse
                {
                    AppCode = $"Chat10.{Chat10ResponseStatusCode.FORBIDDEN}",
                    HttpCode = StatusCodes.Status400BadRequest
                }
        );
    }

    internal static Func<Chat10Response, Chat10HttpResponse> Resolve(
        Chat10ResponseStatusCode statusCode
    )
    {
        if (Equals(_dictionary, default)) Init();

        if (_dictionary.TryGetValue(statusCode, out var response)) return response;

        return _dictionary[statusCode];
    }
}
