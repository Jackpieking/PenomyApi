using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt4;
using System;
using System.Collections.Concurrent;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt4.HttpResponse
{
    public static class Art4HttpResponseManager
    {
        private static ConcurrentDictionary<Art4ResponseStatusCode, Func<Art4Response, Art4HttpResponse>> _dictionary;

        private static void Init()
        {
            _dictionary = new();

            // Add each feature status code with its HttpResponse information.
            _dictionary.TryAdd(
                key: Art4ResponseStatusCode.SUCCESS,
                value: (response) => new()
                {
                    AppCode = Art4HttpResponse.GetAppCode(Art4ResponseStatusCode.SUCCESS),
                    HttpCode = StatusCodes.Status201Created,
                    Body = new DTOs.Art4ResponseDto
                    {
                        ComicId = response.ComicId.ToString()
                    }
                });

            _dictionary.TryAdd(
                key: Art4ResponseStatusCode.DATABASE_ERROR,
                value: (response) => new()
                {
                    AppCode = Art4HttpResponse.GetAppCode(Art4ResponseStatusCode.DATABASE_ERROR),
                    HttpCode = StatusCodes.Status400BadRequest,
                });

            _dictionary.TryAdd(
                key: Art4ResponseStatusCode.INVALID_JSON_SCHEMA_FROM_INPUT_CATEGORIES,
                value: (response) => new()
                {
                    AppCode = Art4HttpResponse.GetAppCode(Art4ResponseStatusCode.INVALID_JSON_SCHEMA_FROM_INPUT_CATEGORIES),
                    HttpCode = StatusCodes.Status400BadRequest,
                });

            _dictionary.TryAdd(
                key: Art4ResponseStatusCode.INVALID_FILE_EXTENSION,
                value: (response) => new()
                {
                    AppCode = Art4HttpResponse.GetAppCode(Art4ResponseStatusCode.INVALID_FILE_EXTENSION),
                    HttpCode = StatusCodes.Status400BadRequest,
                });
        }

        internal static Func<Art4Response, Art4HttpResponse> Resolve(Art4ResponseStatusCode statusCode)
        {
            if (Equals(objA: _dictionary, objB: default))
            {
                Init();
            }

            return _dictionary[statusCode];
        }
    }
}
