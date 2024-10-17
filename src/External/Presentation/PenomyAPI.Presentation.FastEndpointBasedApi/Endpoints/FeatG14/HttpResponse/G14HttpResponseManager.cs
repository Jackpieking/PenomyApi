using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG14;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG14.HttpResponse
{
    public static class G14HttpResponseManager
    {
        private static ConcurrentDictionary<
            G14ResponseStatusCode,
            Func<G14Request, G14Response, G14HttpResponse>
        > _dictionary;

        private static void Init()
        {
            _dictionary = new();

            // Add each feature status code with its HttpResponse information.
            _dictionary.TryAdd(
                key: G14ResponseStatusCode.SUCCESS,
                value: (_, response) =>
                    new()
                    {
                        AppCode = $"G14.{G14ResponseStatusCode.SUCCESS}",
                        HttpCode = StatusCodes.Status200OK,
                    }
            );

            _dictionary.TryAdd(
                key: G14ResponseStatusCode.DATABASE_ERROR,
                value: (_, response) =>
                    new()
                    {
                        AppCode = $"G14.{G14ResponseStatusCode.DATABASE_ERROR}",
                        HttpCode = StatusCodes.Status400BadRequest,
                    }
            );
        }

        internal static Func<G14Request, G14Response, G14HttpResponse> Resolve(
            G14ResponseStatusCode statusCode
        )
        {
            if (Equals(objA: _dictionary, objB: default))
            {
                Init();
            }

            return _dictionary[statusCode];
        }
    }
}
