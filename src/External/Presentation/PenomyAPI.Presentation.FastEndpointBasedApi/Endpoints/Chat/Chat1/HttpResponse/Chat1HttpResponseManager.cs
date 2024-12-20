using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatChat1;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Chat.Chat1.HttpResponse;

public class Chat1HttpResponseManager
{
    private static ConcurrentDictionary<
        Chat1ResponseStatusCode,
        Func<Chat1Response, Chat1HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new ConcurrentDictionary<Chat1ResponseStatusCode, Func<Chat1Response, Chat1HttpResponse>>();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            Chat1ResponseStatusCode.SUCCESS,
            response =>
                new Chat1HttpResponse
                {
                    AppCode = $"Chat1.{Chat1ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK
                }
        );

        _dictionary.TryAdd(
            Chat1ResponseStatusCode.FAILED,
            response =>
                new Chat1HttpResponse
                {
                    AppCode = $"Chat1.{Chat1ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status400BadRequest
                }
        );

        _dictionary.TryAdd(
            Chat1ResponseStatusCode.FORBIDDEN,
            response =>
                new Chat1HttpResponse
                {
                    AppCode = $"Chat1.{Chat1ResponseStatusCode.FORBIDDEN}",
                    HttpCode = StatusCodes.Status403Forbidden
                }
        );

        _dictionary.TryAdd(
            Chat1ResponseStatusCode.UN_AUTHORIZED,
            response =>
                new Chat1HttpResponse
                {
                    AppCode = $"Chat1.{Chat1ResponseStatusCode.UN_AUTHORIZED}",
                    HttpCode = StatusCodes.Status401Unauthorized
                }
        );

        _dictionary.TryAdd(
            Chat1ResponseStatusCode.INVALID_FILE_EXTENSION,
            response =>
                new Chat1HttpResponse
                {
                    AppCode = $"Chat1.{Chat1ResponseStatusCode.INVALID_FILE_EXTENSION}",
                    HttpCode = StatusCodes.Status400BadRequest
                }
        );

        _dictionary.TryAdd(
            Chat1ResponseStatusCode.INVALID_FILE_FORMAT,
            response =>
                new Chat1HttpResponse
                {
                    AppCode = $"Chat1.{Chat1ResponseStatusCode.INVALID_FILE_FORMAT}",
                    HttpCode = StatusCodes.Status400BadRequest
                }
        );
        _dictionary.TryAdd(
            Chat1ResponseStatusCode.MAXIMUM_IMAGE_FILE_SIZE,
            response =>
                new Chat1HttpResponse
                {
                    AppCode = $"Chat1.{Chat1ResponseStatusCode.MAXIMUM_IMAGE_FILE_SIZE}",
                    HttpCode = StatusCodes.Status400BadRequest
                }
        );
    }

    internal static Func<Chat1Response, Chat1HttpResponse> Resolve(
        Chat1ResponseStatusCode statusCode
    )
    {
        if (Equals(_dictionary, default)) Init();

        if (_dictionary.TryGetValue(statusCode, out var response)) return response;

        return _dictionary[statusCode];
    }
}
