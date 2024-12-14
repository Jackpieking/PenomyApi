using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.Chat3;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Chat.Chat3.DTOs;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Chat.Chat3.HttpResponse;

public class Chat3HttpResponseManager
{
    private static ConcurrentDictionary<Chat3ResponseStatusCode, Func<Chat3Response, Chat3HttpResponse>> _dictionary;

    private static void Init()
    {
        _dictionary = new ConcurrentDictionary<Chat3ResponseStatusCode, Func<Chat3Response, Chat3HttpResponse>>();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            Chat3ResponseStatusCode.SUCCESS,
            response => new Chat3HttpResponse
            {
                AppCode = Chat3HttpResponse.GetAppCode(Chat3ResponseStatusCode.SUCCESS),
                HttpCode = StatusCodes.Status201Created,
                Body = new Chat3ResponseDto
                {
                    MessageId = response.MessageId.ToString()
                }
            });

        _dictionary.TryAdd(
            Chat3ResponseStatusCode.DATABASE_ERROR,
            response => new Chat3HttpResponse
            {
                AppCode = Chat3HttpResponse.GetAppCode(Chat3ResponseStatusCode.DATABASE_ERROR),
                HttpCode = StatusCodes.Status500InternalServerError
            });


        _dictionary.TryAdd(
            Chat3ResponseStatusCode.FAILED,
            response => new Chat3HttpResponse
            {
                AppCode = Chat3HttpResponse.GetAppCode(Chat3ResponseStatusCode.FAILED),
                HttpCode = StatusCodes.Status400BadRequest
            });

        _dictionary.TryAdd(
            Chat3ResponseStatusCode.GROUP_NOT_FOUND,
            response => new Chat3HttpResponse
            {
                AppCode = Chat3HttpResponse.GetAppCode(Chat3ResponseStatusCode.GROUP_NOT_FOUND),
                HttpCode = StatusCodes.Status400BadRequest
            });
        _dictionary.TryAdd(
            Chat3ResponseStatusCode.USER_NOT_MEMBER,
            response => new Chat3HttpResponse
            {
                AppCode = Chat3HttpResponse.GetAppCode(Chat3ResponseStatusCode.USER_NOT_MEMBER),
                HttpCode = StatusCodes.Status400BadRequest
            });
    }

    internal static Func<Chat3Response, Chat3HttpResponse> Resolve(Chat3ResponseStatusCode statusCode)
    {
        if (Equals(_dictionary, default)) Init();

        if (_dictionary.TryGetValue(statusCode, out var response)) return response;

        return _dictionary[Chat3ResponseStatusCode.FAILED];
    }
}
