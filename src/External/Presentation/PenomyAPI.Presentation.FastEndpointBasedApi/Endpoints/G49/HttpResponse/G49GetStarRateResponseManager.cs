using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG49;
using PenomyAPI.App.FeatG49.OtherHandlers;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G49.HttpResponse;

public class G49GetStarRateResponseManager
{
    private static ConcurrentDictionary<
        G49ResponseStatusCode,
        Func<G49StarRateRequest, G49StarRateResponse, G49GetStarRateHttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new ConcurrentDictionary<G49ResponseStatusCode, Func<G49StarRateRequest, G49StarRateResponse,
            G49GetStarRateHttpResponse>>();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            G49ResponseStatusCode.SUCCESS,
            (_, response) =>
                new G49GetStarRateHttpResponse
                {
                    AppCode = $"G49.{G49ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK
                }
        );

        _dictionary.TryAdd(
            G49ResponseStatusCode.FAILED,
            (_, response) =>
                new G49GetStarRateHttpResponse
                {
                    AppCode = $"G49.{G49ResponseStatusCode.FAILED}",
                    HttpCode = StatusCodes.Status500InternalServerError
                }
        );
        _dictionary.TryAdd(
            G49ResponseStatusCode.NOT_FOUND,
            (_, response) =>
                new G49GetStarRateHttpResponse
                {
                    AppCode = $"G49.{G49ResponseStatusCode.NOT_FOUND}",
                    HttpCode = StatusCodes.Status404NotFound
                }
        );
        _dictionary.TryAdd(
            G49ResponseStatusCode.UNAUTHORIZED,
            (_, response) =>
                new G49GetStarRateHttpResponse
                {
                    AppCode = $"G49.{G49ResponseStatusCode.UNAUTHORIZED}",
                    HttpCode = StatusCodes.Status401Unauthorized
                }
        );
    }

    internal static Func<G49StarRateRequest, G49StarRateResponse, G49GetStarRateHttpResponse> Resolve(
        G49ResponseStatusCode statusCode
    )
    {
        if (Equals(_dictionary, default)) Init();

        return _dictionary[statusCode];
    }
}
