using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt4;
using System;
using System.Collections.Concurrent;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG3.HttpResponse
{
    public static class FeatArt4HttpResponseManager
    {
        private static ConcurrentDictionary<
            FeatArt4ResponseStatusCode,
            Func<FeatArt4Request, FeatArt4Response, FeatArt4HttpResponse>> _dictionary;

        private static void Init()
        {
            _dictionary = new();

            // Add each feature status code with its HttpResponse information.
            _dictionary.TryAdd(
                key: FeatArt4ResponseStatusCode.SUCCESS,
                value: (_, response) => new()
                {
                    AppCode = $"Art4.{FeatArt4ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                });

            _dictionary.TryAdd(
                key: FeatArt4ResponseStatusCode.DATABASE_ERROR,
                value: (_, response) => new()
                {
                    AppCode = $"Art4.{FeatArt4ResponseStatusCode.DATABASE_ERROR}",
                    HttpCode = StatusCodes.Status400BadRequest,
                });
        }

        internal static Func<FeatArt4Request, FeatArt4Response, FeatArt4HttpResponse> Resolve(
            FeatArt4ResponseStatusCode statusCode)
        {
            if (Equals(objA: _dictionary, objB: default))
            {
                Init();
            }

            return _dictionary[statusCode];
        }
    }
}
