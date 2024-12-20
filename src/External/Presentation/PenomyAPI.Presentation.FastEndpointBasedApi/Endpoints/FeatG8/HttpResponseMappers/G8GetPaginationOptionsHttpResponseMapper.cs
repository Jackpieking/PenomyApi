using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG8;
using PenomyAPI.App.FeatG8.OtherHandlers;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG8.HttpResponse;
using System;
using System.Collections.Concurrent;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG8.HttpResponseMappers;

public static class G8GetPaginationOptionsHttpResponseMapper
{
    private static ConcurrentDictionary<
        G8ResponseStatusCode,
        Func<G8GetPaginationOptionsResponse, G8GetPaginationOptionsHttpResponse>> _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: G8ResponseStatusCode.SUCCESS,
            value: (response) => new()
            {
                AppCode = G8ResponseStatusCode.SUCCESS.ToString(),
                HttpCode = StatusCodes.Status200OK,
                Body = response
            });

        _dictionary.TryAdd(
            key: G8ResponseStatusCode.NOT_FOUND,
            value: (response) => new()
            {
                AppCode = G8ResponseStatusCode.NOT_FOUND.ToString(),
                HttpCode = StatusCodes.Status404NotFound,
            });
    }

    private static Func<G8GetPaginationOptionsResponse, G8GetPaginationOptionsHttpResponse> Resolve(
        G8ResponseStatusCode statusCode)
    {
        if (Equals(objA: _dictionary, objB: default))
        {
            Init();
        }

        if (_dictionary.TryGetValue(statusCode, out var response))
        {
            return response;
        }

        return _dictionary[G8ResponseStatusCode.NOT_FOUND];
    }

    internal static G8GetPaginationOptionsHttpResponse Map(G8GetPaginationOptionsResponse featureResponse)
    {
        return Resolve(featureResponse.StatusCode).Invoke(featureResponse);
    }
}
