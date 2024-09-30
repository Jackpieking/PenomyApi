using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt4;
using System;
using System.Collections.Concurrent;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG3.HttpResponse
{
    public static class Art4HttpResponseManager
    {
        private static ConcurrentDictionary<
            Art4ResponseStatusCode,
            Func<Art4Request, Art4Response, Art4HttpResponse>> _dictionary;

        private static void Init()
        {
            _dictionary = new();

            // Add each feature status code with its HttpResponse information.
            _dictionary.TryAdd(
                key: Art4ResponseStatusCode.SUCCESS,
                value: (_, response) => new()
                {
                    AppCode = $"Art4.{Art4ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                });

            _dictionary.TryAdd(
                key: Art4ResponseStatusCode.DATABASE_ERROR,
                value: (_, response) => new()
                {
                    AppCode = $"Art4.{Art4ResponseStatusCode.DATABASE_ERROR}",
                    HttpCode = StatusCodes.Status400BadRequest,
                });
        }

        internal static Func<Art4Request, Art4Response, Art4HttpResponse> Resolve(
            Art4ResponseStatusCode statusCode)
        {
            if (Equals(objA: _dictionary, objB: default))
            {
                Init();
            }

            return _dictionary[statusCode];
        }
    }
}
