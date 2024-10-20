using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG15;
using System;
using System.Collections.Concurrent;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG15.HttpResponse
{
    public class G15HttpResponseManager
    {
        private static ConcurrentDictionary<
        G15ResponseStatusCode,
        Func<G15Request, G15Response, G15HttpResponse>
    > _dictionary;

        private static void Init()
        {
            _dictionary = new();

            // Add each feature status code with its HttpResponse information.
            _dictionary.TryAdd(
                key: G15ResponseStatusCode.SUCCESS,
                value: (_, response) =>
                    new()
                    {
                        AppCode = $"G15.{G15ResponseStatusCode.SUCCESS}",
                        HttpCode = StatusCodes.Status200OK,
                    }
            );

            _dictionary.TryAdd(
                key: G15ResponseStatusCode.FAILED,
                value: (_, response) =>
                    new()
                    {
                        AppCode = $"G15.{G15ResponseStatusCode.FAILED}",
                        HttpCode = StatusCodes.Status500InternalServerError,
                    }
            );
            _dictionary.TryAdd(
                key: G15ResponseStatusCode.INVALID_REQUEST,
                value: (_, response) =>
                    new()
                    {
                        AppCode = $"G15.{G15ResponseStatusCode.INVALID_REQUEST}",
                        HttpCode = StatusCodes.Status400BadRequest,
                    }
            );
            _dictionary.TryAdd(
                key: G15ResponseStatusCode.NOT_FOUND,
                value: (_, response) =>
                    new()
                    {
                        AppCode = $"G15.{G15ResponseStatusCode.NOT_FOUND}",
                        HttpCode = StatusCodes.Status404NotFound,
                    }
            );
        }

        internal static Func<G15Request, G15Response, G15HttpResponse> Resolve(
            G15ResponseStatusCode statusCode
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
