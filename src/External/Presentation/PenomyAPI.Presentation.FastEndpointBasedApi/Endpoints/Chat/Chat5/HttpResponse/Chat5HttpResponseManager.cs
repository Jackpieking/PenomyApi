using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.Chat5;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Chat.Chat5.DTOs;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Chat.Chat5.HttpResponse;

public class Chat5HttpResponseManager
{
    private static ConcurrentDictionary<Chat5ResponseStatusCode, Func<Chat5Response, Chat5HttpResponse>> _dictionary;

    private static void Init()
    {
        _dictionary = new ConcurrentDictionary<Chat5ResponseStatusCode, Func<Chat5Response, Chat5HttpResponse>>();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            Chat5ResponseStatusCode.SUCCESS,
            response => new Chat5HttpResponse
            {
                AppCode = Chat5ResponseStatusCode.SUCCESS.ToString(),
                HttpCode = StatusCodes.Status201Created,
                Body = new Chat5ResponseDto
                {
                    isSuccess = response.isSuccess
                }
            });


        _dictionary.TryAdd(
            Chat5ResponseStatusCode.FAILED,
            response => new Chat5HttpResponse
            {
                AppCode = Chat5ResponseStatusCode.FAILED.ToString(),
                HttpCode = StatusCodes.Status400BadRequest
            });

        _dictionary.TryAdd(
            Chat5ResponseStatusCode.NOT_EXIST,
            response => new Chat5HttpResponse
            {
                AppCode = Chat5ResponseStatusCode.NOT_EXIST.ToString(),
                HttpCode = StatusCodes.Status400BadRequest
            });
    }

    internal static Func<Chat5Response, Chat5HttpResponse> Resolve(Chat5ResponseStatusCode statusCode)
    {
        if (Equals(_dictionary, default)) Init();

        if (_dictionary.TryGetValue(statusCode, out var response)) return response;

        return _dictionary[Chat5ResponseStatusCode.FAILED];
    }
}
