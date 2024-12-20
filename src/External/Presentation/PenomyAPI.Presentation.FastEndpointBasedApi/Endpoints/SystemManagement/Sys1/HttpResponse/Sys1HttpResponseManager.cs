using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.Sys1;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Sys1.HttpResponse;

public class Sys1HttpResponseManager
{
    private static ConcurrentDictionary<
        Sys1ResponseStatusCode,
        Func<Sys1Response, Sys1HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary =
            new ConcurrentDictionary<
                Sys1ResponseStatusCode,
                Func<Sys1Response, Sys1HttpResponse>
            >();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            Sys1ResponseStatusCode.SUCCESS,
            response => new Sys1HttpResponse
            {
                AppCode = Sys1HttpResponse.GetAppCode(Sys1ResponseStatusCode.SUCCESS),
                HttpCode = StatusCodes.Status201Created,
                Body = new Sys1Response { AccountId = response.AccountId },
            }
        );

        _dictionary.TryAdd(
            Sys1ResponseStatusCode.DATABASE_ERROR,
            response => new Sys1HttpResponse
            {
                AppCode = Sys1HttpResponse.GetAppCode(Sys1ResponseStatusCode.DATABASE_ERROR),
                HttpCode = StatusCodes.Status500InternalServerError,
            }
        );
    }

    internal static Func<Sys1Response, Sys1HttpResponse> Resolve(Sys1ResponseStatusCode statusCode)
    {
        if (Equals(_dictionary, default))
            Init();

        if (_dictionary.TryGetValue(statusCode, out var response))
            return response;

        return _dictionary[Sys1ResponseStatusCode.FAILED];
    }
}
