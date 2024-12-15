using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM34;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM34.DTOs;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM34.HttpResponse;

public class SM34HttpResponseManager
{
    private static ConcurrentDictionary<
        SM34ResponseStatusCode,
        Func<SM34Response, SM34HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary =
            new ConcurrentDictionary<
                SM34ResponseStatusCode,
                Func<SM34Response, SM34HttpResponse>
            >();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            SM34ResponseStatusCode.SUCCESS,
            response => new SM34HttpResponse
            {
                AppCode = SM34HttpResponse.GetAppCode(SM34ResponseStatusCode.SUCCESS),
                HttpCode = StatusCodes.Status201Created,
                Body = new SM34ResponseDto { GroupPostId = response.UserPostId.ToString() },
            }
        );

        _dictionary.TryAdd(
            SM34ResponseStatusCode.DATABASE_ERROR,
            response => new SM34HttpResponse
            {
                AppCode = SM34HttpResponse.GetAppCode(SM34ResponseStatusCode.DATABASE_ERROR),
                HttpCode = StatusCodes.Status500InternalServerError,
            }
        );

        _dictionary.TryAdd(
            SM34ResponseStatusCode.FILE_SERVICE_ERROR,
            response => new SM34HttpResponse
            {
                AppCode = SM34HttpResponse.GetAppCode(SM34ResponseStatusCode.FILE_SERVICE_ERROR),
                HttpCode = StatusCodes.Status500InternalServerError,
            }
        );

        _dictionary.TryAdd(
            SM34ResponseStatusCode.INVALID_FILE_EXTENSION,
            response => new SM34HttpResponse
            {
                AppCode = SM34HttpResponse.GetAppCode(
                    SM34ResponseStatusCode.INVALID_FILE_EXTENSION
                ),
                HttpCode = StatusCodes.Status400BadRequest,
            }
        );

        _dictionary.TryAdd(
            SM34ResponseStatusCode.INVALID_FILE_FORMAT,
            response => new SM34HttpResponse
            {
                AppCode = SM34HttpResponse.GetAppCode(SM34ResponseStatusCode.INVALID_FILE_FORMAT),
                HttpCode = StatusCodes.Status400BadRequest,
            }
        );

        _dictionary.TryAdd(
            SM34ResponseStatusCode.FILE_SIZE_IS_EXCEED_THE_LIMIT,
            response => new SM34HttpResponse
            {
                AppCode = SM34HttpResponse.GetAppCode(
                    SM34ResponseStatusCode.FILE_SIZE_IS_EXCEED_THE_LIMIT
                ),
                HttpCode = StatusCodes.Status400BadRequest,
            }
        );

        _dictionary.TryAdd(
            SM34ResponseStatusCode.USER_PROFILE_NOT_FOUND,
            response => new SM34HttpResponse
            {
                AppCode = SM34HttpResponse.GetAppCode(
                    SM34ResponseStatusCode.USER_PROFILE_NOT_FOUND
                ),
                HttpCode = StatusCodes.Status400BadRequest,
            }
        );
    }

    internal static Func<SM34Response, SM34HttpResponse> Resolve(SM34ResponseStatusCode statusCode)
    {
        if (Equals(_dictionary, default))
            Init();

        if (_dictionary.TryGetValue(statusCode, out var response))
            return response;

        return _dictionary[SM34ResponseStatusCode.FILE_SERVICE_ERROR];
    }
}
