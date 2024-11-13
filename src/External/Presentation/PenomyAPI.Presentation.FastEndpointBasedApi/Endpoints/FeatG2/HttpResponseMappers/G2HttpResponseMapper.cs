using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG2;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG2.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG2.HttpResponses;
using System;
using System.Collections.Concurrent;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG2.HttpResponseMappers;

public static class G2HttpResponseMapper
{
    private static ConcurrentDictionary<
        G2ResponseAppCode,
        Func<G2Response, G2HttpResponse>> _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: G2ResponseAppCode.SUCCESS,
            value: (response) => new()
            {
                AppCode = G2HttpResponse.GetAppCode(G2ResponseAppCode.SUCCESS),
                HttpCode = StatusCodes.Status200OK,
                Body = G2RecommendedArtworkItemResponseDto.MapFrom(response.TopRecommendedArtworks)
            });

        _dictionary.TryAdd(
            key: G2ResponseAppCode.DATABASE_ERROR,
            value: (response) => new()
            {
                AppCode = G2HttpResponse.GetAppCode(G2ResponseAppCode.DATABASE_ERROR),
                HttpCode = StatusCodes.Status500InternalServerError,
            });
    }

    private static Func<G2Response, G2HttpResponse> Resolve(
        G2ResponseAppCode statusCode)
    {
        if (Equals(objA: _dictionary, objB: default))
        {
            Init();
        }

        if (_dictionary.TryGetValue(statusCode, out var response))
        {
            return response;
        }

        return _dictionary[G2ResponseAppCode.DATABASE_ERROR];
    }

    internal static G2HttpResponse Map(G2Response featureResponse)
    {
        return Resolve(featureResponse.AppCode).Invoke(featureResponse);
    }
}
