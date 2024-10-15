using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.G13;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG13.HttpResponse
{
    public static class G13HttpResponseManager
    {
        private static ConcurrentDictionary<
            G13ResponseStatusCode,
            Func<G13Request, G13Response, G13HttpResponse>
        > _dictionary;

        private static void Init()
        {
            _dictionary = new();

            // Add each feature status code with its HttpResponse information.
            _dictionary.TryAdd(
                key: G13ResponseStatusCode.SUCCESS,
                value: (_, response) =>
                    new()
                    {
                        AppCode = $"G3.{G13ResponseStatusCode.SUCCESS}",
                        HttpCode = StatusCodes.Status200OK,
                    }
            );

            _dictionary.TryAdd(
                key: G13ResponseStatusCode.DATABASE_ERROR,
                value: (_, response) =>
                    new()
                    {
                        AppCode = $"G3.{G13ResponseStatusCode.DATABASE_ERROR}",
                        HttpCode = StatusCodes.Status400BadRequest,
                    }
            );
        }

        internal static Func<G13Request, G13Response, G13HttpResponse> Resolve(
            G13ResponseStatusCode statusCode
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
