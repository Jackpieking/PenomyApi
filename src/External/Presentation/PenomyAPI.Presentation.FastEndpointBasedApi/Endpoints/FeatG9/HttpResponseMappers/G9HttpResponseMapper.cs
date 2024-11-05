using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG9;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG9.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG9.HttpResponses;
using System;
using System.Collections.Concurrent;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG9.HttpResponseMappers;

public static class G9HttpResponseMapper
{
    private static ConcurrentDictionary<
        G9ResponseAppCode,
        Func<G9Response, G9HttpResponse>> _dictionary;

    private static void Init()
    {
        _dictionary = new();

        // Add each feature status code with its HttpResponse information.
        _dictionary.TryAdd(
            key: G9ResponseAppCode.SUCCESS,
            value: (response) => new()
            {
                AppCode = G9HttpResponse.GetAppCode(G9ResponseAppCode.SUCCESS),
                HttpCode = StatusCodes.Status200OK,
                Body = G9ResponseDto.MapFrom(response)
            });

        _dictionary.TryAdd(
            key: G9ResponseAppCode.CHAPTER_IS_NOT_FOUND,
            value: (response) => new()
            {
                AppCode = G9HttpResponse.GetAppCode(G9ResponseAppCode.CHAPTER_IS_NOT_FOUND),
                HttpCode = StatusCodes.Status404NotFound,
            });
    }

    private static Func<G9Response, G9HttpResponse> Resolve(
        G9ResponseAppCode statusCode)
    {
        if (Equals(objA: _dictionary, objB: default))
        {
            Init();
        }

        if (_dictionary.TryGetValue(statusCode, out var response))
        {
            return response;
        }

        return _dictionary[G9ResponseAppCode.CHAPTER_IS_NOT_FOUND];
    }

    internal static G9HttpResponse Map(G9Response featureResponse)
    {
        return Resolve(featureResponse.AppCode).Invoke(featureResponse);
    }
}
