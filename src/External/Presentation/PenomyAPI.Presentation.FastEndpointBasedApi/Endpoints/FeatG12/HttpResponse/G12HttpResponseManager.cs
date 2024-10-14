using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG12;
using System;
using System.Collections.Concurrent;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG12.HttpResponse
{
    public static class G12HttpResponseManager
    {
        private static ConcurrentDictionary<
            G12ResponseStatusCode,
            Func<G12Request, G12Response, G12HttpResponse>> _dictionary;

        private static void Init()
        {
            _dictionary = new();

            // Add each feature status code with its HttpResponse information.
            _dictionary.TryAdd(
                key: G12ResponseStatusCode.SUCCESS,
                value: (_, response) => new()
                {
                    AppCode = $"G12.{G12ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                });

            _dictionary.TryAdd(
                key: G12ResponseStatusCode.DATABASE_ERROR,
                value: (_, response) => new()
                {
                    AppCode = $"G12.{G12ResponseStatusCode.DATABASE_ERROR}",
                    HttpCode = StatusCodes.Status400BadRequest,
                });
        }

        internal static Func<G12Request, G12Response, G12HttpResponse> Resolve(
            G12ResponseStatusCode statusCode)
        {
            if (Equals(objA: _dictionary, objB: default))
            {
                Init();
            }

            return _dictionary[statusCode];
        }
    }
}
