using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG10;
using System;
using System.Collections.Concurrent;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG10.HttpResponse
{
    public static class G10HttpResponseManager
    {
        private static ConcurrentDictionary<
            G10ResponseStatusCode,
            Func<G10Request, G10Response, G10HttpResponse>> _dictionary;

        private static void Init()
        {
            _dictionary = new();

            // Add each feature status code with its HttpResponse information.
            _dictionary.TryAdd(
                key: G10ResponseStatusCode.SUCCESS,
                value: (_, response) => new()
                {
                    AppCode = $"G10.{G10ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                });

            _dictionary.TryAdd(
                key: G10ResponseStatusCode.DATABASE_ERROR,
                value: (_, response) => new()
                {
                    AppCode = $"G10.{G10ResponseStatusCode.DATABASE_ERROR}",
                    HttpCode = StatusCodes.Status400BadRequest,
                });
        }

        internal static Func<G10Request, G10Response, G10HttpResponse> Resolve(
            G10ResponseStatusCode statusCode)
        {
            if (Equals(objA: _dictionary, objB: default))
            {
                Init();
            }

            return _dictionary[statusCode];
        }
    }
}
