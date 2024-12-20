using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM12;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM12.DTOs;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM12.HttpResponse;

public class SM12HttpResponseManager
{
    private static ConcurrentDictionary<SM12ResponseStatusCode, Func<SM12Response, SM12HttpResponse>> _dictionary;

    private static void Init()
    {
        _dictionary = new ConcurrentDictionary<SM12ResponseStatusCode, Func<SM12Response, SM12HttpResponse>>();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            SM12ResponseStatusCode.SUCCESS,
            response => new SM12HttpResponse
            {
                AppCode = SM12HttpResponse.GetAppCode(SM12ResponseStatusCode.SUCCESS),
                HttpCode = StatusCodes.Status201Created,
                Body = new SM12ResponseDto
                {
                    UserPostId = response.UserPostId.ToString()
                }
            });

        _dictionary.TryAdd(
            SM12ResponseStatusCode.DATABASE_ERROR,
            response => new SM12HttpResponse
            {
                AppCode = SM12HttpResponse.GetAppCode(SM12ResponseStatusCode.DATABASE_ERROR),
                HttpCode = StatusCodes.Status500InternalServerError
            });

        _dictionary.TryAdd(
            SM12ResponseStatusCode.FILE_SERVICE_ERROR,
            response => new SM12HttpResponse
            {
                AppCode = SM12HttpResponse.GetAppCode(SM12ResponseStatusCode.FILE_SERVICE_ERROR),
                HttpCode = StatusCodes.Status500InternalServerError
            });

        _dictionary.TryAdd(
            SM12ResponseStatusCode.INVALID_FILE_EXTENSION,
            response => new SM12HttpResponse
            {
                AppCode = SM12HttpResponse.GetAppCode(SM12ResponseStatusCode.INVALID_FILE_EXTENSION),
                HttpCode = StatusCodes.Status400BadRequest
            });

        _dictionary.TryAdd(
            SM12ResponseStatusCode.INVALID_FILE_FORMAT,
            response => new SM12HttpResponse
            {
                AppCode = SM12HttpResponse.GetAppCode(SM12ResponseStatusCode.INVALID_FILE_FORMAT),
                HttpCode = StatusCodes.Status400BadRequest
            });


        _dictionary.TryAdd(
            SM12ResponseStatusCode.FILE_SIZE_IS_EXCEED_THE_LIMIT,
            response => new SM12HttpResponse
            {
                AppCode = SM12HttpResponse.GetAppCode(SM12ResponseStatusCode.FILE_SIZE_IS_EXCEED_THE_LIMIT),
                HttpCode = StatusCodes.Status400BadRequest
            });

        _dictionary.TryAdd(
            SM12ResponseStatusCode.USER_PROFILE_NOT_FOUND,
            response => new SM12HttpResponse
            {
                AppCode = SM12HttpResponse.GetAppCode(SM12ResponseStatusCode.USER_PROFILE_NOT_FOUND),
                HttpCode = StatusCodes.Status400BadRequest
            });
    }

    internal static Func<SM12Response, SM12HttpResponse> Resolve(SM12ResponseStatusCode statusCode)
    {
        if (Equals(_dictionary, default)) Init();

        if (_dictionary.TryGetValue(statusCode, out var response)) return response;

        return _dictionary[SM12ResponseStatusCode.FILE_SERVICE_ERROR];
    }
}
