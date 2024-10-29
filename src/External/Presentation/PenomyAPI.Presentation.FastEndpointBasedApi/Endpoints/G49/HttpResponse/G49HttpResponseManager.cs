using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG49;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G49.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG49.HttpResponse;

public class G49HttpResponseManager
{
    private static ConcurrentDictionary<
        G49ResponseStatusCode,
        Func<G49Request, G49Response, G49HttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new ConcurrentDictionary<G49ResponseStatusCode, Func<G49Request, G49Response, G49HttpResponse>>();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            G49ResponseStatusCode.SUCCESS,
            (_, response) =>
                new G49HttpResponse
                {
                    AppCode = $"G49.{G49ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK
                }
        );

        _dictionary.TryAdd(
            G49ResponseStatusCode.FAILED,
            (_, response) =>
                new G49HttpResponse
                {
                    AppCode = $"G49.{G49ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status500InternalServerError
                }
        );
        _dictionary.TryAdd(
            G49ResponseStatusCode.NOT_FOUND,
            (_, response) =>
                new G49HttpResponse
                {
                    AppCode = $"G49.{G49ResponseStatusCode.NOT_FOUND}",
                    HttpCode = StatusCodes.Status404NotFound
                }
        );
    }

    internal static Func<G49Request, G49Response, G49HttpResponse> Resolve(
        G49ResponseStatusCode statusCode
    )
    {
        if (Equals(_dictionary, default)) Init();

        return _dictionary[statusCode];
    }
}
