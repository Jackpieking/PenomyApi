using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG5;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5.DTOs;
using System;
using System.Collections.Concurrent;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5.HttpResponse;

public class G5HttpResponseManager
{
    private static ConcurrentDictionary<
        G5ResponseStatusCode,
        Func<G5Request, G5Response, G5HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new ConcurrentDictionary<G5ResponseStatusCode, Func<G5Request, G5Response, G5HttpResponse>>();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            G5ResponseStatusCode.SUCCESS,
            (_, response) =>
                new G5HttpResponse
                {
                    AppCode = $"G5.{G5ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                    Body = G5ResponseDto.MapFrom(response)
                }
        );

        _dictionary.TryAdd(
            G5ResponseStatusCode.FAILED,
            (_, response) =>
                new G5HttpResponse
                {
                    AppCode = $"G5.{G5ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status500InternalServerError
                }
        );
        _dictionary.TryAdd(
            G5ResponseStatusCode.INVALID_REQUEST,
            (_, response) =>
                new G5HttpResponse
                {
                    AppCode = $"G5.{G5ResponseStatusCode.INVALID_REQUEST}",
                    HttpCode = StatusCodes.Status400BadRequest
                }
        );
        _dictionary.TryAdd(
            G5ResponseStatusCode.NOT_FOUND,
            (_, response) =>
                new G5HttpResponse
                {
                    AppCode = $"G5.{G5ResponseStatusCode.NOT_FOUND}",
                    HttpCode = StatusCodes.Status404NotFound
                }
        );
        _dictionary.TryAdd(
            G5ResponseStatusCode.UNAUTHORIZED,
            (_, response) =>
                new G5HttpResponse
                {
                    AppCode = $"G5.{G5ResponseStatusCode.UNAUTHORIZED}",
                    HttpCode = StatusCodes.Status401Unauthorized
                }
        );
    }

    internal static Func<G5Request, G5Response, G5HttpResponse> Resolve(
        G5ResponseStatusCode statusCode
    )
    {
        if (Equals(_dictionary, default)) Init();

        return _dictionary[statusCode];
    }
}
